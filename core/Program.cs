using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _164253
{
    class Program
    {
        static void Main(string[] args)
        {
            int saisie = 0;
            Boolean loop = true;
            Trainer trainer = new Trainer();
            Question question = new Question();
            Validation startValid = new Validation();

            do
            {
                //printed menu
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("-----------------------------------------");
                Console.Out.WriteLine("|\tPROJECT OF 2NET \t \t|");
                Console.Out.WriteLine("-----------------------------------------");
                Console.ResetColor();
                Console.Out.WriteLine("1. Create new Trainer");
                Console.Out.WriteLine("2. Create new Questions");
                Console.Out.WriteLine("3. start the validation");
                Console.Out.WriteLine("4. Show each Trainers");
                Console.Out.WriteLine("5. Show each Questions");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Out.WriteLine("------------------------");
                Console.Out.WriteLine("Make your choice : 1,2, ... 5 or 0 for exit");
                Console.Out.WriteLine("------------------------");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.Write("Your choice : ");
                Console.ResetColor();
                try
                {
                    saisie = Convert.ToInt32(Console.In.ReadLine());
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Fatal error :");
                    Console.Out.WriteLine(e);
                }

                switch (saisie)
                {
                    case 0 :
                        loop = false;
                        break;
                    case 1 :
                        trainer.add();
                        break;
                    case 2 :
                        question.Add();
                        break;
                    case 3 :
                        Console.Out.Write("ID of teacher :");
                        startValid.StartValidation(Convert.ToInt32(Console.In.ReadLine()));
                        break;
                    case 4 :
                        trainer.show();
                        break;
                    case 5 :
                        Console.Out.Write("Show by course(0) or all(1) : ");
                        if (Convert.ToInt32(Console.In.ReadLine()) == 0)
                        {
                            Console.Out.Write("Search of course :");
                            question.show(Console.In.ReadLine());
                        }
                        else
                            question.show();
                        break;
                }

                //Check confirmation to erase console
                Console.Out.WriteLine("Erase console(1) or no(just press key ENTER) :");
                if(Console.In.ReadLine()=="1")
                    System.Console.Clear();
            } while (loop);

        }
    }
}
