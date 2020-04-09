using System;
using System.Threading;
using System.Threading.Tasks;

namespace Covid19Routine
{
    public interface ITimeSupplier
    {
        TimeSpan FromMinutes(double minutes);
        TimeSpan FromHours(double hours);
        int GetMinutesLeft(TimeSpan s);
    }

    public class SimulatedTimeSupplier : ITimeSupplier
    {
        public TimeSpan FromMinutes(double minutes) => TimeSpan.FromSeconds(minutes) / 60.0;
        public TimeSpan FromHours(double hours) => TimeSpan.FromSeconds(hours);
        public int GetMinutesLeft(TimeSpan s) => (int)Math.Ceiling(s.TotalSeconds * 60);
    }

    public class RealTimeSupplier : ITimeSupplier
    {
        public TimeSpan FromMinutes(double minutes) => TimeSpan.FromMinutes(minutes);
        public TimeSpan FromHours(double hours) => TimeSpan.FromHours(hours);
        public int GetMinutesLeft(TimeSpan s) => (int)Math.Ceiling(s.TotalMinutes);
    }

    class Program
    {
        public static ITimeSupplier TimeSupplier;

        static void Main(string[] args)
        {
            TimeSupplier = new SimulatedTimeSupplier();

            while (COVID_19())
            {
                StayAtHome();
            }
        }

        private static bool COVID_19()
        {
            // let's face it... this thing isn't ever going to end :(
            return true;
        }

        public static void StayAtHome()
        {
            WakeUp();

            try
            {
                Exercise();
            }
            catch
            {
                AddReminder(DateTime.Now.AddDays(1), "Do some exercise");
            }

            var daytime = new CancellationTokenSource();
            CancellationToken ct = daytime.Token;

            DrinkAsync(ct);

            Eat();

            Code();

            QualityTimeWithFamily();

            var kidsSleep = BedTimeRoutineAsync();

            daytime.Cancel();

            var sleep = SleepAsync();

            Task.WaitAll(sleep, kidsSleep);
        }

        private static Task SleepAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("ZzZzZzZzZz...");
                Thread.Sleep(TimeSupplier.FromHours(8));
                Console.WriteLine("... ZzZ ... zZz ... *yawn*");
            });
        }

        private static Task BedTimeRoutineAsync()
        {
            Console.WriteLine("Kids... bed time");
            Thread.Sleep(TimeSupplier.FromMinutes(5));

            Console.WriteLine("Kids... go to bed!");
            Thread.Sleep(TimeSupplier.FromMinutes(5));

            Console.WriteLine("KIDS!!!");
            Thread.Sleep(TimeSupplier.FromMinutes(10));

            Console.WriteLine("KIDS GO TO BED RIGHT NOW!@#%!@#%");
            Thread.Sleep(TimeSupplier.FromMinutes(10));

            Console.WriteLine("Kids... PLEEEEEEEEASE FOR THE LOVE OF GOD WILL YOU PLEASE GO TO BED!? MOMMY AND DADDY NEED QUIET TIME!");
            Thread.Sleep(TimeSupplier.FromMinutes(10));

            return Task.Run(() =>
            {
                var rnd = new Random();

                var peaceAndQuietLeft = TimeSupplier.FromHours(9);
                var victim = new[] { "MOMMY", "DADDY" };
                var wakeCounter = 0;

                while (peaceAndQuietLeft.Ticks > 0)
                {
                    var maxMinutesLeft = Math.Max(TimeSupplier.GetMinutesLeft(peaceAndQuietLeft), 60);

                    var uninterruptedSleepTime = rnd.Next(60, maxMinutesLeft);
                    var uninterruptedSleepTimeSpan = TimeSupplier.FromMinutes(uninterruptedSleepTime);

                    Console.WriteLine($"{maxMinutesLeft} more minutes of sleep for kiddos. I think we'll sleep for {uninterruptedSleepTime} minutes this time...");
                    Thread.Sleep(uninterruptedSleepTimeSpan);
                    Console.WriteLine($"{victim[wakeCounter++ % 2]}!!!");

                    var timeUntilBackToSleep = TimeSupplier.FromMinutes(5);

                    peaceAndQuietLeft -= uninterruptedSleepTimeSpan;
                    peaceAndQuietLeft -= timeUntilBackToSleep;
                }
            });
        }

        private static void QualityTimeWithFamily()
        {
            Console.WriteLine("Screens on!");
            Thread.Sleep(TimeSupplier.FromMinutes(45));

            Console.WriteLine("Screens off!");
            Thread.Sleep(TimeSupplier.FromMinutes(5));

            Console.WriteLine("...");
            Thread.Sleep(TimeSupplier.FromMinutes(2));

            Console.WriteLine("Screens on!");
            Thread.Sleep(TimeSupplier.FromMinutes(60));
        }

        private static void Code()
        {
            Console.WriteLine("http://www.reddit.com/");
            Thread.Sleep(TimeSupplier.FromHours(7));

            Console.WriteLine("> code");
            Thread.Sleep(TimeSupplier.FromHours(1));
        }

        private static void Eat()
        {
            Console.WriteLine("Nom nom nom.");
            Thread.Sleep(TimeSupplier.FromMinutes(30));
        }

        private static void WakeUp()
        {
            Console.WriteLine("Ugh, here we go again...");
        }

        private static Task DrinkAsync(CancellationToken ct)
        {
            return Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    ConsumeBeer();
                }
            }, ct);
        }

        private static void ConsumeBeer()
        {
            // take time to enjoy the beer (and try not to get too drunk throughout the day)
            Console.WriteLine("*glug glug glug*");
            Thread.Sleep(TimeSupplier.FromMinutes(60));
        }

        private static void AddReminder(DateTime addDays, string doSomeExercise)
        {
            // NOOP - You're not gonna remember a post-it note anyway
        }

        private static void Exercise()
        {
            Console.WriteLine("One and two and one and two and ...");
            Thread.Sleep(TimeSupplier.FromMinutes(30));
        }
    }
}
