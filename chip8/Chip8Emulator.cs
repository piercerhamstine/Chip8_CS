
namespace Emulator;

public class Chip8Emulator{
    private Chip8 chip8;
    private string rom;
    
    public Chip8Emulator(string romPath, bool debugFlag = false){
        chip8 = new Chip8();
        rom = romPath;
        chip8.DebugFlag = debugFlag;
    }

    public void Run(){
        chip8.LoadRom(rom);
        while(!chip8.ExitFlag){
            chip8.ExecuteCycle();

            if(chip8.DebugFlag){
                Console.ReadKey();
            }
        }
    }
}