namespace Emulator;

enum OpCodes: ushort{

    x00E0 = 0x00E0,
    x00EE = 0x00EE,
    // Jump to location NNN
    x1NNN = 0x1000,
    // Call subroutine at NNN
    x2NNN = 0x2000,
    // Skip next instruction if value of register:x == NN 
    x3XNN = 0x3000,
    // Skip next instruction if value of register:x != NN
    x4XNN = 0x4000,
    // Skip next instruction if register:x == register:y
    x5XY0 = 0x5000,
    // Setx register:x to value of NN
    x6XNN = 0x6000,
    // Add value of NN to register:x (No carry flag)
    x7XNN = 0x7000,
    // Set register:x to value of register:y
    x8XY0 = 0x8000,
    // Set register:x to register:x | register:y (bitwise OR)
    x8XY1 = 0x8001,
    // Set register:x to register:x & register:y (bitwise AND)
    x8XY2 = 0x8002,
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
    // Load value of VX into Delay Timer
    LDDTVX = 0xF015,
    LDFVX = 0xF029,
    // Store from v0-vx in memory startin at index register
    LDIV0VX = 0xF055,
    // Add value of I and VX, store in I
    ADDIVX = 0xF01E,
    // Load value of memory starting at I into v0-vx
    LDVXI = 0xF065,
}