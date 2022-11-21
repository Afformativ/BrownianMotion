using System;
using System.Linq;
using System.Threading;

namespace BrownianMotion
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = 8;
            var k = 50;
            var p = 0.6;

            var withLock = true;

            var sleepSec = 10;
            var checkStatEverySec = 2;

            var crystal = new Crystal(n, k, p, withLock);

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            TimerCallback callback = new TimerCallback(WriteState);

            crystal.StartSimulation(token);
            Timer timer = new Timer(callback, crystal, TimeSpan.Zero, TimeSpan.FromSeconds(checkStatEverySec));
            Thread.Sleep(TimeSpan.FromSeconds(sleepSec));

            tokenSource.Cancel();

            timer.Change(Timeout.Infinite, Timeout.Infinite);

            Console.WriteLine("================= End simulation =========================");
            WriteState(crystal);
            Console.WriteLine("==================================== Total count =======================================");
            Console.WriteLine(crystal.CrystalState.Sum());
        }

        public static void WriteState(object crystalObj)
        {
            var crystal = (Crystal)crystalObj;
            var state = crystal.CrystalState.ToList();

            var line = state.Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}");
            Console.WriteLine(line);
        }
    }
}