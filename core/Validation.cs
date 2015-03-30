using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _164253
{
    class Validation
    {
        private string _course;
        private string _allRanking;

        public void StartValidation(int Id)
        {
            //Initialisation
            Database1Entities data = new Database1Entities();
            List<int> tabQuest = new List<int>();
            int idQuest=0;
            string rankQuest = null;
            string ects = null;

            while (data.Teacher.Find(Id) == null)
            {
                Console.Out.Write("Bad ID. enter correct ID : ");
                Id = Convert.ToInt32(Console.In.ReadLine());
            }

            do
            {
                Console.Out.WriteLine("-------------------------------");
                Console.Out.WriteLine("Courses : ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine( data.Courses.Find( data.Teacher.Find( Id ).IdCoursesWould ).CodeMat );
                Console.ResetColor();
                Console.Out.WriteLine("-------------------------------");
                Console.Out.Write("Select the course you would to validate : ");
                _course = Console.In.ReadLine();
            } while (data.Courses.Find(_course)==null);

            Console.Out.WriteLine("Select the questions by the id (0 for stop select) :");
            foreach (Questions source in data.Questions.ToArray())
            {
                if (source.CodeMat == _course)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Out.WriteLine("ID : " + source.Id);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.WriteLine("\tQuestion : " + source.Question);
                    Console.ResetColor();
                }
            }

            do
            {
                Console.Out.Write("ID question : ");
                idQuest = Convert.ToInt32(Console.In.ReadLine());
                if (idQuest != 0)
                    tabQuest.Add(idQuest);
            } while (idQuest != 0);

            //Notation question
            foreach (int question in tabQuest)
            {
                Console.Out.WriteLine("Question : "+data.Questions.Find(question).Question);
                Console.Out.Write("Rating{A,B,C} : ");
                rankQuest = Console.In.ReadLine();
                
                //check bonus or important
                if (data.Questions.Find(question).Bonus == true)
                {
                    if (rankQuest == "A")
                        _allRanking += "AA";
                    else if (rankQuest == "B")
                        _allRanking += "A";
                    else
                        _allRanking += "C";
                }
                else
                {
                    if (rankQuest == "C")
                        _allRanking += "CC";
                    else
                        _allRanking += rankQuest;
                }
                
            }

            //notation certification
            if (data.Teacher.Find(Id).IdCertification != null)
            {
                if (data.Certification.Find(data.Teacher.Find(Id).IdCertification).Grade == "Professional")
                    _allRanking += 'A';
                else if (data.Certification.Find(data.Teacher.Find(Id).IdCertification).Grade == "Academic")
                    _allRanking += 'B';
                else
                    _allRanking += 'C';
            }
            else
            {
                _allRanking += 'C';
            }

            //Notation  ECTS Credits
            Console.Out.Write("Validation of ECTS credits : ");
            ects = Console.In.ReadLine();
            if (ects == "RESIT")
                _allRanking += 'C';
            else
            {
                if (Convert.ToInt32(ects) >= 15)
                    _allRanking += 'A';
                else if (Convert.ToInt32(ects) >= 10)
                    _allRanking += 'B';
                else
                    _allRanking += 'C';
            }

            //Notation work experience
            Console.Out.Write("For work experience(A,B,C) : ");
            _allRanking += Console.In.ReadLine();

            if (Validate(_allRanking))
            {
                Console.Out.WriteLine("The teacher is validate !");
                SendMail(data.Teacher.Find(Id));

            }
            else
            {
                Console.Out.WriteLine("The teacher is not valide.");
            }

        }

        private Boolean Validate(string notation)
        {
            int nbA = 0;
            int nbC = 0;
            Boolean validate = false;

            for (int i = 0; i < notation.Length; i++)
            {
                if (notation[i] == 'A')
                    nbA++;
                else if (notation[i] == 'C')
                    nbC++;
            }

            if (nbC == 0 && nbA >= 1)
                validate = true;
            else if (nbC == 1 && nbA >= 2)
                validate = true;

            return validate;
        }

        private void SendMail(Teacher teacherValidate)
        {
            string signature = "The Full Prof of " + _course;
            string fromMail,mdp,server;
            int port = 587;

            Console.Out.WriteLine("Mail d'envois :");
            fromMail = Console.In.ReadLine();

            Console.Out.WriteLine("Le mot de passe : ");
            mdp = Console.In.ReadLine();

            Console.Out.WriteLine("L'adresse IP ou le nom du serveur smtp :");
            server = Console.In.ReadLine();

            Console.Out.WriteLine("Le port d'utilisation :");
            port = Convert.ToInt32(Console.In.ReadLine());

            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(teacherValidate.Email);
                message.Subject = "[ECTS " + _course + "] : Validation obtained";
                message.From = new System.Net.Mail.MailAddress(fromMail);
                message.Body = "You are now a teacher.\n" + signature;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(server, port);

                smtp.Credentials = new System.Net.NetworkCredential(fromMail,mdp);

                smtp.EnableSsl = true;

                smtp.Send(message);

                Console.Out.WriteLine("Email envoyer !");
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Erreur : "+e);
            }


        }
    }
}
