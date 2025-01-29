// CD74HCT151E Multiplexer Control Pins (Connected to ProMicro F4-F7)
#define MUX1_S0 4  // F4
#define MUX1_S1 5  // F5 
#define MUX1_S2 6  // F6
#define MUX1_INPUT 7  // F7/A0

#define MUX2_S0 4  // Shares control lines with MUX1
#define MUX2_S1 5
#define MUX2_S2 6
#define MUX2_INPUT 8  // B4

// Additional Loose Button Pin
#define EXTRA_BUTTON 9  // B5

// 74HC595 Shift Register Pins for LEDs
#define LED_LATCH 10    // B6
#define LED_CLOCK 16    // B2
#define LED_DATA 14     // B3

// Button and LED Configuration
// Add these LED mapping tables at the top with your other constants
const byte LED_MAP_REGISTER1[8] = {0, 1, 2, 3, 6, 7, 5, 4};     // First register: 1,2,3,4,7,8,6,5
const byte LED_MAP_REGISTER2[8] = {7, 6, 4, 5, 3, 2, 1, 0};     // Second register: 21,20,18,19,17,16,14,11
const byte LED_MAP_REGISTER3[5] = {0, 3, 2, 1, 4};              // Third register: 13,10,15,12,9

const int TOTAL_BUTTONS = 17;
const int TOTAL_LEDS = 21;  // 16 regular LEDs + 5 LEDs for special button
const String BUTTON_IDS[TOTAL_BUTTONS] = {
    "CAM01", "CAM02", "CAM03", "CAM04", "CAM05", 
    "CAM06", "CAM07", "CAM08", "CAM09", "CAM10",
    "CAM11", "CAM12", "CAM13", "CAM14", "CAM15",
    "Lure", "LockDown"
};

// State tracking
bool buttonStates[TOTAL_BUTTONS] = {false};
bool ledStates[TOTAL_LEDS] = {false};
bool lastButtonStates[TOTAL_BUTTONS] = {false};
unsigned long lastDebounceTime[TOTAL_BUTTONS] = {0};
bool ledStatesChanged = false;
int currentActiveButton = -1;  // Tracks which button's LED is currently lit

const unsigned long debounceDelay = 200;

void setup() {
    // Multiplexer Control Pins (shared between both multiplexers)
    pinMode(MUX1_S0, OUTPUT);
    pinMode(MUX1_S1, OUTPUT);
    pinMode(MUX1_S2, OUTPUT);
    pinMode(MUX1_INPUT, INPUT_PULLUP);
    pinMode(MUX2_INPUT, INPUT_PULLUP);

    // Extra Button Pin
    pinMode(EXTRA_BUTTON, INPUT_PULLUP);

    // LED Shift Register Pins
    pinMode(LED_LATCH, OUTPUT);
    pinMode(LED_CLOCK, OUTPUT);
    pinMode(LED_DATA, OUTPUT);

    // Serial Communication
    Serial.begin(115200);

    // Initialize LED states (all off)
    UpdateShiftRegisters(0, 0, 0);
}

bool ReadMultiplexer(int s0Pin, int s1Pin, int s2Pin, int inputPin, byte channel) {
    digitalWrite(s0Pin, bitRead(channel, 0));
    digitalWrite(s1Pin, bitRead(channel, 1));
    digitalWrite(s2Pin, bitRead(channel, 2));
    delayMicroseconds(10);
    return digitalRead(inputPin) == LOW;
}

bool debounce(int buttonIndex) {
    unsigned long currentTime = millis();
    if (currentTime - lastDebounceTime[buttonIndex] > debounceDelay && !lastButtonStates[buttonIndex]) {
        lastDebounceTime[buttonIndex] = currentTime;
        lastButtonStates[buttonIndex] = true;
        return true;
    }
    return false;
}

void loop() {
    // Scan first multiplexer (first 8 buttons)
    for (byte i = 0; i < 8; i++) {
        bool buttonPressed = ReadMultiplexer(MUX1_S0, MUX1_S1, MUX1_S2, MUX1_INPUT, i);
        if (!buttonPressed) {
            lastButtonStates[i] = false;
        }
        if (buttonPressed && debounce(i)) {
            HandleButtonPress(i);
        }
    }

    // Scan second multiplexer (next 7 buttons including Lure)
    for (byte i = 0; i < 8; i++) {
        if ((i + 8) >= 15) break; // Stop before Lure
        bool buttonPressed = ReadMultiplexer(MUX2_S0, MUX2_S1, MUX2_S2, MUX2_INPUT, i);
        if (!buttonPressed) {
            lastButtonStates[i + 8] = false;
        }
        if (buttonPressed && debounce(i + 8)) {
            HandleButtonPress(i + 8);
        }
    }

    // Check Lure button
    bool lurePressed = ReadMultiplexer(MUX2_S0, MUX2_S1, MUX2_S2, MUX2_INPUT, 7);
    if (!lurePressed) {
        lastButtonStates[15] = false;
    }
    if (lurePressed && debounce(15)) {
        HandleLureButton();
    }

    // Check LockDown button (Extra Button)
    bool lockdownPressed = (digitalRead(EXTRA_BUTTON) == LOW);
    if (!lockdownPressed) {
        lastButtonStates[16] = false;
    }
    if (lockdownPressed && debounce(16)) {
        HandleLockDownButton();
    }

    // Check for serial commands
    if (Serial.available() > 0) {
        HandleUnityCommand();
    }

    // Update LEDs if states changed
    if (ledStatesChanged) {
        UpdateLEDs();
        ledStatesChanged = false;
    }

    delay(10);
}

void HandleButtonPress(int channelIndex) {
    if (channelIndex >= 15) return; // Only handle regular buttons here
    
    Serial.println(BUTTON_IDS[channelIndex]);
    
    // Turn off all regular LEDs first
    for (int i = 0; i < 15; i++) {
        ledStates[i] = false;
    }
    
    // Turn on the LED corresponding to this button
    ledStates[channelIndex] = true;
    currentActiveButton = channelIndex;
    
    ledStatesChanged = true;
}

void HandleLureButton() {
    Serial.println("Lure");
    buttonStates[15] = !buttonStates[15];  // Toggle Lure state
    ledStates[15] = buttonStates[15];      // Turn LED on/off based on state
    ledStatesChanged = true;
}

void HandleLockDownButton() {
    Serial.println("LockDown");
    buttonStates[16] = !buttonStates[16];  // Toggle LockDown state
    ledStates[16] = buttonStates[16];      // Turn LED on/off based on state
    ledStatesChanged = true;
}

void UpdateLEDs() {
    byte ledStates1 = 0;  // First 8 LEDs
    byte ledStates2 = 0;  // Next 8 LEDs
    byte ledStates3 = 0;  // Last 5 LEDs

    // First 8 LEDs (Register 1)
    for (int i = 0; i < 8; i++) {
        if (ledStates[i]) {
            ledStates1 |= (1 << LED_MAP_REGISTER1[i]);
        }
    }

    // Next 8 LEDs (Register 2)
    for (int i = 8; i < 16; i++) {
        if (ledStates[i]) {
            ledStates2 |= (1 << LED_MAP_REGISTER2[i - 8]);
        }
    }

    // Last 5 LEDs (Register 3)
    for (int i = 16; i < TOTAL_LEDS; i++) {
        if (ledStates[i]) {
            ledStates3 |= (1 << LED_MAP_REGISTER3[i - 16]);
        }
    }

    UpdateShiftRegisters(ledStates1, ledStates2, ledStates3);
}

void UpdateShiftRegisters(byte leds1, byte leds2, byte leds3) {
    digitalWrite(LED_LATCH, LOW);
    shiftOut(LED_DATA, LED_CLOCK, MSBFIRST, leds3);
    shiftOut(LED_DATA, LED_CLOCK, MSBFIRST, leds2);
    shiftOut(LED_DATA, LED_CLOCK, MSBFIRST, leds1);
    digitalWrite(LED_LATCH, HIGH);
}

void HandleUnityCommand() {
    String command = Serial.readStringUntil('\n');
    command.trim();

    if (command.length() == 0) {
        Serial.println("ERROR: Empty command");
        return;
    }

    if (command.startsWith("LED:")) {
        int ledIndex = command.substring(4).toInt();
        if (ledIndex >= 0 && ledIndex < TOTAL_LEDS) {
            if (ledIndex < 15) {  // Regular LEDs
                // Turn off all regular LEDs first
                for (int i = 0; i < 15; i++) {
                    ledStates[i] = false;
                }
                // Turn on only the requested LED
                ledStates[ledIndex] = true;
                currentActiveButton = ledIndex;
            } else {  // Special LEDs (Lure/LockDown)
                ledStates[ledIndex] = !ledStates[ledIndex];  // Toggle the special LED
                buttonStates[ledIndex] = ledStates[ledIndex];
            }
            ledStatesChanged = true;
        } else {
            Serial.println("ERROR: Invalid LED index");
        }
    } else if (command.startsWith("CONFIRM:")) {
        Serial.println("Command Received");
    } else {
        Serial.println("ERROR: Unrecognized command");
    }
}