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

void setup() {
  // put your setup code here, to run once:
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
}

void loop() {
  // put your main code here, to run repeatedly:
  for(byte i = 0; i < 8; i++){
    digitalWrite (MUX1_S0, bitRead(i, 0));
    digitalWrite (MUX1_S1, bitRead(i, 1));
    digitalWrite (MUX1_S2, bitRead(i, 2));

    Serial.println(String(i) + " <> " + String(digitalRead(MUX1_INPUT)));
    Serial.println(String(i + 8) + " <> " + String(digitalRead(MUX2_INPUT)));
  }
  Serial.println("16 <> " + String(digitalRead(EXTRA_BUTTON)));
}