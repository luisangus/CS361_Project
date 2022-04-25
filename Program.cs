///////////////////////////////////////////////////////////////////////////////////////////
// Program: SendEmail
// Author: Luis R Gonzalez
// Date: 4/21/22
// Description: The purpose of this microservice is to turn a file into an HTML table and 
// email it. The user interface will ask the user for all the information.
///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;

namespace SendEmail
{
    class Program
    {
        static int Main(string[] args)
        {
            List<List<string>> inputFile = new List<List<string>>();

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter the file path for the file you want to email (.csv format only)");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            string inputFilePath = Console.ReadLine();
            Console.WriteLine("");
            string HTMLtable = "";

            //read inputfile
            inputFile = readCSV(inputFilePath);

            // add HTML Table Headers
            HTMLtable = addTableHeaders(inputFile[0], HTMLtable);

            // add HTML Table Rows
            for (int i = 1; i < inputFile.Count; i++)
            {
                HTMLtable = addTableRow(inputFile[i], HTMLtable);
            }


            // asking for email and password to send the email
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter your Gmail address");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            string email = Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter your password");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            char[] psw = new char[100]; //Assign the max length of password you want
            ConsoleKeyInfo keyinfo = new ConsoleKeyInfo();
            for (int i = 0; i < psw.Length; i++)
            {
                keyinfo = Console.ReadKey(true);
                if (!keyinfo.Key.Equals(ConsoleKey.Enter))
                {
                    psw[i] = keyinfo.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    break;
                }
            }
            string pwd = new string(psw);
            //Console.WriteLine("\nPassword is " + pwd);
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter recepient email address: ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            string to = Console.ReadLine();

            // email HTML table
            smtpEmail(email, pwd, to, "Subject", HTMLtable);

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("The microservice ran succesfully!");

            return 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // This function receives a List<string> (which is like an array), and also a string that we will 
        // keep adding/appending HTML rows to it.
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string addTableRow(List<string> row, string appendToString)
        {
            //creates a new html row of cells for the table
            appendToString += "<tr>";
            // This will add each cell in the row
            for (int j = 0; j < row.Count; j++)
            {
                appendToString += "<td>" + row[j] + "</td>";
            }
            //closes the html row
            appendToString += "</tr>";
            // returns back the table with the added row
            return appendToString;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // This function receives a List<string> (which is like an array), and also a string that we will 
        // add the table Headers
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string addTableHeaders(List<string> row, string appendToString)
        {
            //creates a new html row of cells for the table
            appendToString += "<tr>";
            // This will add each cell in the row
            for (int j = 0; j < row.Count; j++)
            {
                appendToString += "<th>" + row[j] + "</th>";
            }
            //closes the html row
            appendToString += "</tr>";
            // returns back the table with the added row
            return appendToString;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // Function: smtpEmail
        // Description: The purpose of this function is to send an email with the specific
        // parameters passed to this function
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static int smtpEmail(string fromEmail, string passw, string To, string subject, string emailBody)
        {

            SmtpClient mailClient = new SmtpClient("aspmx.l.google.com", 25); // port 587 or port 25
            mailClient.Credentials = new System.Net.NetworkCredential(fromEmail, passw);
            mailClient.EnableSsl = true;
            //add each recepient to meeting notice
            string[] recipients;
            recipients = To.Split(';');
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            foreach (string recipient in recipients)
            {
                string r1 = recipient;
                r1 = recipient.Trim();
                if (r1 != "")
                {
                    message.To.Add(r1);
                }
            }
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = emailBody;

            mailClient.Timeout = 2000000;
            mailClient.Send(message);
            message = null;
            mailClient = null;

            return 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Function: readCSV
        // Description: The purpose of this function is to return a list of lists that contains
        // the file in memory.
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static List<List<string>> readCSV(string csvFile)
        {

            List<List<string>> csvList = new List<List<string>>();
            StreamReader logReader = new StreamReader(new FileStream(csvFile,
                                    FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            while (!logReader.EndOfStream)
            {
                csvList.Add(logReader.ReadLine().Split(',').ToList());
            }
            logReader.Close();

            return csvList;
        }


    }
}
