using System;
using System.Collections.Generic;

namespace NearestPoints
{
    struct Ans
    {
        public double distance;
        public int p1, p2;
        public Ans (double d, int _p1, int _p2)
        {
            distance = d;
            p1 = _p1;
            p2 = _p2;
        }
    }
    class Points
    {
        public List<Point> arr;
        private int X_MAX, Y_MAX; // 坐标最大值
        private double INF = 1e30;
        public int p1, p2; // 最近的两个点的坐标
        public Points(int num = 0, int X_MAX = 500, int Y_MAX = 500)
        {
            arr = new List<Point>(num);
            this.X_MAX = X_MAX;
            this.Y_MAX = Y_MAX;
            addPointsRandomly(num);
        }

        public void addPointsRandomly(int num)
        {
            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                addPoint(random.Next(0, X_MAX), random.Next(0, Y_MAX));
            }
        }

        public void addPoint(double x, double y)
        {
            arr.Add(new Point(x, y));
        }

        private double distance(Point a, Point b)
        {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }

        private int cmp(Point a, Point b)
        {
            if (a.x > b.x) return 1;
            else if (a.x < b.x) return -1;
            return 0;
        }

        /// <summary>
        /// 复杂度为nlgn的分治方法
        /// </summary>
        /// <returns></returns>
        public double getSmallestDistant()
        {
            arr.Sort(cmp);
            List<int> t = new List<int>();
            return dfs(0, arr.Count - 1, t).distance;
        }

        private Ans dfs(int l, int r, List<int> t)
        {
            if (r <= l) {
                t.Add(l);
                return new Ans(INF, -1, -1);
            }
            List<int> t1 = new List<int>(), t2 = new List<int>();
            int m = (l + r) / 2;
            double MIN;
            Ans ans1 = dfs(l, m, t1), ans2 = dfs(m + 1, r, t2);
            if (ans1.distance < ans2.distance)
            {
                MIN = ans1.distance;
                p1 = ans1.p1; p2 = ans1.p2;
            } else
            {
                MIN = ans2.distance;
                p1 = ans2.p1; p2 = ans2.p2;
            }
            int len1 = t1.Count, len2 = t2.Count, i = 0, j = 0;
            while (i < len1 || j < len2)
            {
                if (j >= len2 || (i < len1 && arr[t1[i]].y < arr[t2[j]].y))
                    t.Add(t1[i++]);
                else t.Add(t2[j++]);
            }
            for (i = 0; i < t.Count; i++)
                for (j = i + 1; j <= i + 7 && j < t.Count;j++) // 7 次比较
                {
                    int x = t[i], y = t[j];
                    double d = distance(arr[x], arr[y]);
                    if (d < MIN)
                    {
                        MIN = d;
                        p1 = x; p2 = y;
                    }
                }
            return new Ans(MIN, p1, p2);
        }

        /// <summary>
        /// n^2复杂度的普通方法
        /// </summary>
        /// <returns></returns>
        public double ordinaryMethod()
        {
            double MIN = INF;
            for(int i = 0; i < arr.Count; i++)
                for (int j = i + 1; j < arr.Count; j++)
                {
                    MIN = Math.Min(MIN, distance(arr[i], arr[j]));
                }
            return MIN;
        }
    }
}
