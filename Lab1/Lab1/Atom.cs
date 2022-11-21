using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BrownianMotion
{
    class Atom
    {
        private int _currentIndex;
        private int _n;
        private double _p;
        private Random _rand;

        public delegate void AtomEventHandler(object sender, AtomEventArgs e);
        public event AtomEventHandler AtomEvent;
        public int Number { get; set; }

        public Atom(int currentIndex, int n, double p)
        {
            _currentIndex = currentIndex;
            _n = n;
            _p = p;
            _rand = new Random();
        }


        public void StartMove(CancellationToken stop)
        {
            while (!stop.IsCancellationRequested)
            {
                var probability = _rand.NextDouble();

                // move right
                if (probability > _p)
                {
                    if (_currentIndex + 1 < _n)
                    {
                        _currentIndex += 1;
                        AtomEvent(this, new AtomEventArgs { from = _currentIndex - 1, to = _currentIndex });
                    }
                }

                // move left
                else
                {
                    if (_currentIndex > 0)
                    {
                        _currentIndex -= 1;
                        AtomEvent(this, new AtomEventArgs { from = _currentIndex + 1, to = _currentIndex });
                    }
                }
            }
        }
    }

    public class AtomEventArgs : EventArgs
    {
        public int from { get; set; }
        public int to { get; set; }
    }
}