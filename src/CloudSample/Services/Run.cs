using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudSample.Services
{
    public static class Run
    {
        private const int Second = 1000;

        public static void TightLoop(int retries, int interval, Action routine) 
        {
            int retryCount = retries;
            int backOffInterval = interval;
            int currentCount = 0;

            do
            {
                try 
                {
                    routine();
                    break;
                }
                catch(Exception ex)
                {
                    if (++currentCount >= retryCount) break;
                    Thread.Sleep(backOffInterval * Second);
                }
            } while (true);
        }

        public static void WithRandomInterval(int retries, int intervalMin, int intervalMax, Action routine)
        {
            Random randomGen = new Random();
            int retryCount = retries;
            int minInterval = intervalMin;
            int maxInterval = intervalMax;
            int backOffInterval = 0;
            int currentCount = 0;
            
            do
            {
                try
                {
                    routine();
                    break;
                }
                catch (Exception ex)
                {
                    if (++currentCount >= retryCount) break;
                    backOffInterval = randomGen.Next(minInterval, maxInterval);
                    Thread.Sleep(backOffInterval * Second);
                }
            } while (true);
        }

        public static void WithProgressBackOff(int retries, int intervalMin, int intervalMax, Action routine)
        {
            int retryCount = retries;
            int minInterval = intervalMin;
            int maxInterval = intervalMax;
            int backOffInterval = intervalMin;
            int exponent = 2;
            int currentCount = 0;

            do
            {
                try
                {
                    routine();
                    break;
                }
                catch (Exception ex)
                {
                    if (++currentCount >= retryCount) break;
                    Thread.Sleep(backOffInterval * Second);
                    backOffInterval = Math.Min(maxInterval, backOffInterval * exponent);
                }
            } while (true);

        }
    }
}
