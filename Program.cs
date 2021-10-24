using System;
using System.Collections;
using System.Globalization;

namespace Unrvld_task
{
    class Program
    {

        /// <summary>
        /// The application is in working state, definetely had fun doing it, learned some new things about date parsing
        /// Unfortunately I was pretty busy so didn't add any secret easter egg
        /// hidden to the program but I think you will still enjoy looking through it
        /// Could not figure out the van example (one that went over multiple days, the program works with car and bike methods because they are single day only)
        /// Something in the loop is looping too much when trying to do multiple day ones and I could not find the cause in time :(
        /// *Insert the larpasComfy emote here* https://www.frankerfacez.com/emoticon/457104-larpasComfy
        /// 
        /// Discord:    asapaska#7225
        /// Email:      povilas.gec@gmail.com
        /// Github:     https://github.com/povka
        /// Twitter:    https://twitter.com/Povka69
        /// Twitch:     https://twitch.tv/asapaska
        /// 
        /// </summary>

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, thank you for using Glasgow Congestion Charge calculator");
            Console.WriteLine("Please enter your vehicle congestion data to calculate an estimated cost for travel");
            Console.WriteLine("Enter data in this format: VehicleType: DD/MM/YYYY HH:MM - DD/MM/YYYY HH:MM");
            Console.WriteLine("Vehicle types include Car, Motorbike and Van");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            ReadData();
        }

        static void ReadData()
        {
            string input = Console.ReadLine();
            string[] splits = input.Split(' ');
            Boolean colon = splits[0].Contains(":");
            if (colon == false || splits[3] != "-")
            {
                Console.WriteLine("The input data is in incorrect format");
            }
            else
            {
                string vehicle = splits[0].Remove(splits[0].Length - 1);
                var dateFormat = "dd/MM/yyyy HH:mm";

                string dateTime1 = splits[1] + " " + splits[2];
                string dateTime2 = splits[4] + " " + splits[5];

                var TimeFormat = "HH:mm";
                DateTime JustTime1 = DateTime.ParseExact(splits[2], TimeFormat, null);
                DateTime JustTime2 = DateTime.ParseExact(splits[5], TimeFormat, null);

                DateTime convertedDate1 = DateTime.ParseExact(dateTime1, dateFormat, null);
                DateTime convertedDate2 = DateTime.ParseExact(dateTime2, dateFormat, null);

                Calculate(vehicle, convertedDate1, convertedDate2, JustTime1, JustTime2);
            }
        }

        static void Calculate(string vehicle, DateTime convertedDate1, DateTime convertedDate2, DateTime JustTime1, DateTime JustTime2)
        {
            double SevenAmToTwelvePm = 2.0;
            double TwelvePmToSevenPm = 2.5;
            double motorbike = 1.0;
            double AmRate = 0.0;
            double PmRate = 0.0;

            int stopper1 = 0;
            int stopper2 = 0;

            double temp1 = 0.0;
            double temp2 = 0.0;
            TimeSpan TimeDivision = convertedDate2 - convertedDate1;
            TimeSpan nightTime = new TimeSpan();
            TimeSpan amTime = new TimeSpan();
            TimeSpan pmTime = new TimeSpan();
            TimeSpan amTime1;
            TimeSpan pmTime1;
            TimeSpan passMinute = new TimeSpan(0, 0, 1, 0);
            TimeSpan passDay = new TimeSpan(1, 0, 0, 0);
            DateTime date;
            DateTime temp;
            for (date = convertedDate1; date <= convertedDate2; date = date.AddMinutes(1))
            {
                DayOfWeek dw1 = date.DayOfWeek;
                string dw = dw1.ToString();

                if (vehicle == "Car" || vehicle == "Van")
                {
                    while (convertedDate1 < convertedDate2)
                    {

                        if (convertedDate1.Hour > 19 && convertedDate1.Hour <= 7 && (convertedDate1.DayOfWeek.ToString() != "Saturday" || convertedDate1.DayOfWeek.ToString() != "Sunday"))
                        {
                            nightTime = nightTime.Add(passMinute);
                            
                            

                        }
                        if (convertedDate1.Hour > 7 && convertedDate1.Hour < 12 && (convertedDate1.DayOfWeek.ToString() != "Saturday" || convertedDate1.DayOfWeek.ToString() != "Sunday"))
                        {
                            if (stopper1 != 0)
                            {
                                amTime = amTime.Add(passMinute);
                                temp1 = amTime.Hours * SevenAmToTwelvePm;
                                temp1 += amTime.Minutes * (SevenAmToTwelvePm / 60);
                            }
                            stopper1++;


                        }
                        if (convertedDate1.Hour >= 12 && convertedDate1.Hour <= 19 && (convertedDate1.DayOfWeek.ToString() != "Saturday" || convertedDate1.DayOfWeek.ToString() != "Sunday"))
                        {
                            if (stopper2 != 0)
                            {
                                pmTime = pmTime.Add(passMinute);
                                temp2 = pmTime.Hours * TwelvePmToSevenPm;
                                temp2 += pmTime.Minutes * (TwelvePmToSevenPm / 60);
                            }
                            stopper2++;
                        }




                        convertedDate1 = convertedDate1.Add(passMinute);
                    }

                }
                else if (vehicle == "Motorbike")
                {

                    while (convertedDate1 < convertedDate2)
                    {

                        if (convertedDate1.Hour > 19 && convertedDate1.Hour < 7)
                        {
                            nightTime = nightTime.Add(passMinute);
                        }
                        if (convertedDate1.Hour > 7 && convertedDate1.Hour < 12 && (convertedDate1.DayOfWeek.ToString() != "Saturday" || convertedDate1.DayOfWeek.ToString() != "Sunday"))
                        {
                            amTime = amTime.Add(passMinute);
                            temp1 = amTime.Hours * motorbike;
                            temp1 = amTime.Minutes * (motorbike / 60);



                        }
                        if (convertedDate1.Hour >= 12 && convertedDate1.Hour < 19 && (convertedDate1.DayOfWeek.ToString() != "Saturday" || convertedDate1.DayOfWeek.ToString() != "Sunday"))
                        {
                            pmTime = pmTime.Add(passMinute);
                            temp2 = pmTime.Hours * motorbike;
                            temp2 += pmTime.Minutes * (motorbike / 60);


                        }
                        convertedDate1 = convertedDate1.Add(passMinute);
                        
                    }
                }


                AmRate = temp1;
                PmRate = temp2;
                
                stopper1 = 0;
                stopper2 = 0;
            }
                
            
            
            PrintRates(AmRate, PmRate, amTime, pmTime, vehicle);   
        }

        static void PrintRates(double AmRate, double PmRate, TimeSpan AmTime, TimeSpan PmTime, string vehicle)
        {
            decimal morning = (decimal)AmRate;
            decimal day = (decimal)PmRate;
            decimal total = (decimal)(AmRate + PmRate);
            if(vehicle == "Motorbike")
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Charge for ");
                Console.Write(AmTime.Hours);
                Console.Write("h ");
                Console.Write(AmTime.Minutes);
                Console.Write(" (AM Rate):£");
                Console.WriteLine(Truncate(morning, 2));
                Console.Write("Charge for ");
                Console.Write(PmTime.Hours);
                Console.Write("h ");
                Console.Write(PmTime.Minutes);
                Console.Write(" (AM Rate):£");
                Console.WriteLine(Truncate(day, 2));
                Console.Write("Total Charge:£");
                Console.Write(Truncate(total, 2));
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Charge for ");
                Console.Write(AmTime.Hours);
                Console.Write("h ");
                Console.Write(AmTime.Minutes+1);
                Console.Write(" (AM Rate):£");
                Console.WriteLine(Truncate(morning, 2));
                Console.Write("Charge for ");
                Console.Write(PmTime.Hours);
                Console.Write("h ");
                Console.Write(PmTime.Minutes+1);
                Console.Write(" (AM Rate):£");
                Console.WriteLine(Truncate(day, 2));
                Console.Write("Total Charge:£");
                Console.Write(Truncate(total, 2));
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
            
        }

        public static decimal Truncate(decimal d, byte decimals) 
        {
            decimal r = Math.Round(d, decimals);

            if (d > 0 && r > d)
            {
                return r - new decimal(1, 0, 0, false, decimals);
            }
            else if (d < 0 && r < d)
            {
                return r + new decimal(1, 0, 0, false, decimals);
            }

            return r;
        }
    }
}
