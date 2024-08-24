namespace Emulator;

enum OpCodes: uint{

    CLS = 0x00E0,
    RTS = 0x00EE,
    JMP = 0x1000,
    CALLNNN = 0x2000,
    SEVXNN = 0x3000,
    // Skip next instruction if VX != NN
    SNEVXNN = 0x4000,
    U = 0x5000,
    LDVXNN = 0x6000,
    // Add NN to VX, Store in VX
    ADDVXNN = 0x7000,
    SETI = 0xA000,
    DRAW = 0xD000,
}