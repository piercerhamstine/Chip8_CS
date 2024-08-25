namespace Emulator;

enum OpCodes: uint{

    CLS = 0x00E0,
    RTS = 0x00EE,
    JMP = 0x1000,
    CALLNNN = 0x2000,
    SEVXNN = 0x3000,
    // Skip next instruction if VX != NN
    SNEVXNN = 0x4000,
    // Skip next instruction if registers VX == VY
    SEVXVY = 0x5000,
    LDVXNN = 0x6000,
    // Add NN to VX, Store in VX
    ADDVXNN = 0x7000,
    SETVXVY = 0x8000,
    SETVXVYOR = 0x8001,
    SETVXVYAND = 0x8002,
    SETVXVYXOR = 0x8003,
    ADDVXVY = 0x8004,
    SUBVYVX = 0x8005,
    SHIFTVXRIGHT = 0x8006,
    SHIFTVXLEFT = 0x800E,
    // Skip next instruction if VX != VY
    SNEVXVY = 0x9000,
    // Sets I to address NNN
    SETI = 0xA000,
    DRAW = 0xD000,
}