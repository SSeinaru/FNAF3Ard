#define MUX_S0 2
#define MUX_S1 3
#define MUX_S2 4
#define MUX_O1 7
#define MUX_O2 8
#define BTN 9

#define CLK 10
#define LATCH 16
#define SER 14

void setup() {
  // put your setup code here, to run once:
pinMode(MUX_S0, OUTPUT);
pinMode(MUX_S1, OUTPUT);
pinMode(MUX_S2, OUTPUT);

Serial.begin(115200);

pinMode(MUX_O1, INPUT);
pinMode(MUX_O2, INPUT);
pinMode(BTN, INPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  for(byte i = 0; i < 8; i++){
    digitalWrite (MUX_S0, bitRead(i, 0));
    digitalWrite (MUX_S1, bitRead(i, 1));
    digitalWrite (MUX_S2, bitRead(i, 2));

    Serial.println(String(i) + " <> " + String(digitalRead(MUX_O1)));
    Serial.println(String(i + 8) + " <> " + String(digitalRead(MUX_O2)));
  }
  Serial.println("16 <> " + String(digitalRead(BTN)));
}