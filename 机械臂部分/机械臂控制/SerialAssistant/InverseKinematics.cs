using System;
using MatrixTool;

namespace SerialAssistant
{
    class InverseKinematics
    {
        private static double RAD2ANG(double a)
        {
            double b;
            b = a * (180.0 / 3.1415926535898);
            return b;
        }
        private static double ANG2RAD(double a)
        {
            double b;
            b = a * (3.1415926535898 /180.0 );
            return b;
        }
        public static bool IK4DOF_downward(double x, double y, double z, out double[] jointAngle4DOF)   //机械臂逆解
        {
            double a, b;//中间变量
            jointAngle4DOF = new double[4];
            double L1 = 10.5, L2 = 13.5, L3 = 13,error=1;
            double m, n, t, q, p;//中间变量
            double x1, y1, z1,yy;//逆解后正解的值，用以验证
            double ANG2RED = 3.1415926535898 / 180.0;
            double[,] temp = new double[300, 3];
            double maxj3 = 0;//存放最大角度
            int u = 0;//i为有无解标志位，u为有无精确解标志位
            int counter = 0, maxcounter = 0;
            // if (((Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)) > Math.Pow(L1 + L2 + L3, 2))) printf("超出范围");
            bool ifsol = false;
            jointAngle4DOF[0] = Math.Atan2(y, x);
            b = InverseKinematics.RAD2ANG(jointAngle4DOF[0]);
            a = x / Math.Cos(jointAngle4DOF[0]);
            if (x == 0) a = y;
            b = z;
            for (jointAngle4DOF[1] = -90; jointAngle4DOF[1] < 90; jointAngle4DOF[1]++)
            {
                jointAngle4DOF[1] *= ANG2RED;
                double temp1 = (Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(L1, 2) - Math.Pow(L2, 2) - Math.Pow(L3, 2) - 2 * a * L1 * Math.Sin(jointAngle4DOF[1]) - 2 * b * L1 * Math.Cos(jointAngle4DOF[1])) / (2 * L2 * L3);
                if (Math.Abs(temp1) <= 1)
                {
                    jointAngle4DOF[3] = Math.Acos(temp1);
                    if (RAD2ANG(jointAngle4DOF[3]) >= 95 || RAD2ANG(jointAngle4DOF[3]) <= -135) { jointAngle4DOF[1] = InverseKinematics.RAD2ANG(jointAngle4DOF[1]); continue; }
                    m = L2 * Math.Sin(jointAngle4DOF[1]) + L3 * Math.Sin(jointAngle4DOF[1]) * Math.Cos(jointAngle4DOF[3]) + L3 * Math.Cos(jointAngle4DOF[1]) * Math.Sin(jointAngle4DOF[3]);
                    n = L2 * Math.Cos(jointAngle4DOF[1]) + L3 * Math.Cos(jointAngle4DOF[1]) * Math.Cos(jointAngle4DOF[3]) - L3 * Math.Sin(jointAngle4DOF[1]) * Math.Sin(jointAngle4DOF[3]);
                    t = a - L1 * Math.Sin(jointAngle4DOF[1]);
                    p = Math.Pow(Math.Pow(n, 2) + Math.Pow(m, 2), 0.5);
                    q = Math.Asin(m / p);
                    jointAngle4DOF[2] = -(Math.Asin(t / p) - q);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOF[2])) >= 135) { jointAngle4DOF[1] = InverseKinematics.RAD2ANG(jointAngle4DOF[1]); continue; }
                    x1 = (L1 * Math.Sin(jointAngle4DOF[1]) + L2 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2]) + L3 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2] + jointAngle4DOF[3])) * Math.Cos(jointAngle4DOF[0]);
                    y1 = (L1 * Math.Sin(jointAngle4DOF[1]) + L2 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2]) + L3 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2] + jointAngle4DOF[3])) * Math.Sin(jointAngle4DOF[0]);
                    z1 = L1 * Math.Cos(jointAngle4DOF[1]) + L2 * Math.Cos(jointAngle4DOF[1] + jointAngle4DOF[2]) + L3 * Math.Cos(jointAngle4DOF[1] + jointAngle4DOF[2] + jointAngle4DOF[3]);

                    jointAngle4DOF[1] = InverseKinematics.RAD2ANG(jointAngle4DOF[1]);
                    jointAngle4DOF[2] = InverseKinematics.RAD2ANG(jointAngle4DOF[2]);
                    jointAngle4DOF[3] = InverseKinematics.RAD2ANG(jointAngle4DOF[3]);
                    if (x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        counter++;
                        ifsol = true;
                        u = 1;
                        for (int k = 0; k < 3; k++)
                        { temp[counter, k] = jointAngle4DOF[k + 1]; }
                        if (temp[counter, 2] > (maxj3))
                        {
                            maxj3 = temp[counter, 2];
                            maxcounter = counter;
                        }
                    }

                }
                else
                {
                    jointAngle4DOF[1] = RAD2ANG(jointAngle4DOF[1]);

                }

            }

            for (jointAngle4DOF[1] = -90; jointAngle4DOF[1] < 90; jointAngle4DOF[1]++)
            {
                jointAngle4DOF[1] *= ANG2RED;
                double temp1 = (Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(L1, 2) - Math.Pow(L2, 2) - Math.Pow(L3, 2) - 2 * a * L1 * Math.Sin(jointAngle4DOF[1]) - 2 * b * L1 * Math.Cos(jointAngle4DOF[1])) / (2 * L2 * L3);
                if (Math.Abs(temp1) <= 1)
                {
                    jointAngle4DOF[3] = Math.Acos(temp1);
                    if (RAD2ANG(jointAngle4DOF[3]) >= 95 || RAD2ANG(jointAngle4DOF[3]) <= -135) { jointAngle4DOF[1] = InverseKinematics.RAD2ANG(jointAngle4DOF[1]); continue; }
                    m = L2 * Math.Sin(jointAngle4DOF[1]) + L3 * Math.Sin(jointAngle4DOF[1]) * Math.Cos(jointAngle4DOF[3]) + L3 * Math.Cos(jointAngle4DOF[1]) * Math.Sin(jointAngle4DOF[3]);
                    n = L2 * Math.Cos(jointAngle4DOF[1]) + L3 * Math.Cos(jointAngle4DOF[1]) * Math.Cos(jointAngle4DOF[3]) - L3 * Math.Sin(jointAngle4DOF[1]) * Math.Sin(jointAngle4DOF[3]);
                    t = a - L1 * Math.Sin(jointAngle4DOF[1]);
                    p = Math.Pow(Math.Pow(n, 2) + Math.Pow(m, 2), 0.5);
                    q = ANG2RAD(180) - Math.Asin(m / p);
                    jointAngle4DOF[2] = -(Math.Asin(t / p) - q);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOF[2])) >= 135) { jointAngle4DOF[1] = InverseKinematics.RAD2ANG(jointAngle4DOF[1]); continue; }
                    x1 = (L1 * Math.Sin(jointAngle4DOF[1]) + L2 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2]) + L3 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2] + jointAngle4DOF[3])) * Math.Cos(jointAngle4DOF[0]);
                    y1 = (L1 * Math.Sin(jointAngle4DOF[1]) + L2 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2]) + L3 * Math.Sin(jointAngle4DOF[1] + jointAngle4DOF[2] + jointAngle4DOF[3])) * Math.Sin(jointAngle4DOF[0]);
                    z1 = L1 * Math.Cos(jointAngle4DOF[1]) + L2 * Math.Cos(jointAngle4DOF[1] + jointAngle4DOF[2]) + L3 * Math.Cos(jointAngle4DOF[1] + jointAngle4DOF[2] + jointAngle4DOF[3]);

                    jointAngle4DOF[1] = InverseKinematics.RAD2ANG(jointAngle4DOF[1]);
                    jointAngle4DOF[2] = InverseKinematics.RAD2ANG(jointAngle4DOF[2]);
                    jointAngle4DOF[3] = InverseKinematics.RAD2ANG(jointAngle4DOF[3]);
                    if (x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        counter++;
                        ifsol = true;
                        u = 1;
                        for (int k = 0; k < 3; k++)
                        { temp[counter, k] = jointAngle4DOF[k + 1]; }
                        if (temp[counter, 2] > (maxj3))
                        {
                            maxj3 = temp[counter, 2];
                            maxcounter = counter;
                        }
                    }

                }
                else
                {
                    jointAngle4DOF[1] = RAD2ANG(jointAngle4DOF[1]);

                }

            }
            
            jointAngle4DOF[0] = InverseKinematics.RAD2ANG(jointAngle4DOF[0]);
            if (u == 1) //有解选取j1最小的
            {
                jointAngle4DOF[1] = temp[maxcounter, 0];
                jointAngle4DOF[2] = temp[maxcounter, 1];
                jointAngle4DOF[3] = temp[maxcounter, 2];
            }
            return ifsol;
        }

        public static bool IK4DOF_level(double x, double y, double z, out double[] jointAngle4DOF)   //机械臂逆解
        {
            double a, b;//中间变量
            double [] jointAngle4DOFtemp = new double[4];
            jointAngle4DOF = new double[4];
            double L1 = 10.5, L2 = 13.5, L3 = 13, error = 1;
            double m, n, t, q, p;//中间变量
            double x1, y1, z1;//逆解后正解的值，用以验证
            double ANG2RED = 3.1415926535898 / 180.0;
            int u = 0;//i为有无解标志位，u为有无精确解标志位
            // if (((Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)) > Math.Pow(L1 + L2 + L3, 2))) printf("超出范围");
            bool ifsol = false;
            jointAngle4DOFtemp[0] = Math.Atan2(y, x);
            b = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[0]);
            a = x / Math.Cos(jointAngle4DOFtemp[0]);
            if (x == 0) a = y;
            b = z;
            for (jointAngle4DOFtemp[1] = -90; jointAngle4DOFtemp[1] < 90; jointAngle4DOFtemp[1]++)
            {
                jointAngle4DOFtemp[1] *= ANG2RED;
                double temp1 = (Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(L1, 2) - Math.Pow(L2, 2) - Math.Pow(L3, 2) - 2 * a * L1 * Math.Sin(jointAngle4DOFtemp[1]) - 2 * b * L1 * Math.Cos(jointAngle4DOFtemp[1])) / (2 * L2 * L3);
                if (Math.Abs(temp1) <= 1)
                {
                    jointAngle4DOFtemp[3] = -Math.Acos(temp1);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOFtemp[3])) >= 135) { jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]); continue; }
                    m = L2 * Math.Sin(jointAngle4DOFtemp[1]) + L3 * Math.Sin(jointAngle4DOFtemp[1]) * Math.Cos(jointAngle4DOFtemp[3]) + L3 * Math.Cos(jointAngle4DOFtemp[1]) * Math.Sin(jointAngle4DOFtemp[3]);
                    n = L2 * Math.Cos(jointAngle4DOFtemp[1]) + L3 * Math.Cos(jointAngle4DOFtemp[1]) * Math.Cos(jointAngle4DOFtemp[3]) - L3 * Math.Sin(jointAngle4DOFtemp[1]) * Math.Sin(jointAngle4DOFtemp[3]);
                    t = a - L1 * Math.Sin(jointAngle4DOFtemp[1]);
                    p = Math.Pow(Math.Pow(n, 2) + Math.Pow(m, 2), 0.5);
                    q = ANG2RAD(180) - Math.Asin(m / p);
                    jointAngle4DOFtemp[2] = -(Math.Asin(t / p) - q);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOFtemp[2])) >= 135) { jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]); continue; }
                    x1 = (L1 * Math.Sin(jointAngle4DOFtemp[1]) + L2 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3])) * Math.Cos(jointAngle4DOFtemp[0]);
                    y1 = (L1 * Math.Sin(jointAngle4DOFtemp[1]) + L2 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3])) * Math.Sin(jointAngle4DOFtemp[0]);
                    z1 = L1 * Math.Cos(jointAngle4DOFtemp[1]) + L2 * Math.Cos(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Cos(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3]);

                    jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]);
                    jointAngle4DOFtemp[2] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[2]);
                    jointAngle4DOFtemp[3] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[3]);
                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]+ jointAngle4DOFtemp[3] - 90)<3 &&x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 6 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 10 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                }
                else
                {
                    jointAngle4DOFtemp[1] = RAD2ANG(jointAngle4DOFtemp[1]);

                }
            }

            /******************************************************/
            if(!ifsol)
            for (jointAngle4DOFtemp[1] = -90; jointAngle4DOFtemp[1] < 90; jointAngle4DOFtemp[1]++)
            {
                jointAngle4DOFtemp[1] *= ANG2RED;
                double temp1 = (Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(L1, 2) - Math.Pow(L2, 2) - Math.Pow(L3, 2) - 2 * a * L1 * Math.Sin(jointAngle4DOFtemp[1]) - 2 * b * L1 * Math.Cos(jointAngle4DOFtemp[1])) / (2 * L2 * L3);
                if (Math.Abs(temp1) <= 1)
                {
                    jointAngle4DOFtemp[3] = Math.Acos(temp1);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOFtemp[3])) >= 135) { jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]); continue; }
                    m = L2 * Math.Sin(jointAngle4DOFtemp[1]) + L3 * Math.Sin(jointAngle4DOFtemp[1]) * Math.Cos(jointAngle4DOFtemp[3]) + L3 * Math.Cos(jointAngle4DOFtemp[1]) * Math.Sin(jointAngle4DOFtemp[3]);
                    n = L2 * Math.Cos(jointAngle4DOFtemp[1]) + L3 * Math.Cos(jointAngle4DOFtemp[1]) * Math.Cos(jointAngle4DOFtemp[3]) - L3 * Math.Sin(jointAngle4DOFtemp[1]) * Math.Sin(jointAngle4DOFtemp[3]);
                    t = a - L1 * Math.Sin(jointAngle4DOFtemp[1]);
                    p = Math.Pow(Math.Pow(n, 2) + Math.Pow(m, 2), 0.5);
                    q = Math.Asin(m / p);
                    jointAngle4DOFtemp[2] = -(Math.Asin(t / p) - q);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOFtemp[2])) >= 135) { jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]); continue; }
                    x1 = (L1 * Math.Sin(jointAngle4DOFtemp[1]) + L2 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3])) * Math.Cos(jointAngle4DOFtemp[0]);
                    y1 = (L1 * Math.Sin(jointAngle4DOFtemp[1]) + L2 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3])) * Math.Sin(jointAngle4DOFtemp[0]);
                    z1 = L1 * Math.Cos(jointAngle4DOFtemp[1]) + L2 * Math.Cos(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Cos(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3]);

                    jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]);
                    jointAngle4DOFtemp[2] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[2]);
                    jointAngle4DOFtemp[3] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[3]);
                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 3 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 6 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 10 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                }
                else
                {
                    jointAngle4DOFtemp[1] = RAD2ANG(jointAngle4DOFtemp[1]);

                }

            }

            /******************************************************/
            if (!ifsol)
            for (jointAngle4DOFtemp[1] = -90; jointAngle4DOFtemp[1] < 90; jointAngle4DOFtemp[1]++)
            {
                jointAngle4DOFtemp[1] *= ANG2RED;
                double temp1 = (Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(L1, 2) - Math.Pow(L2, 2) - Math.Pow(L3, 2) - 2 * a * L1 * Math.Sin(jointAngle4DOFtemp[1]) - 2 * b * L1 * Math.Cos(jointAngle4DOFtemp[1])) / (2 * L2 * L3);
                if (Math.Abs(temp1) <= 1)
                {
                    jointAngle4DOFtemp[3] = Math.Acos(temp1);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOFtemp[3])) >= 135) { jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]); continue; }
                    m = L2 * Math.Sin(jointAngle4DOFtemp[1]) + L3 * Math.Sin(jointAngle4DOFtemp[1]) * Math.Cos(jointAngle4DOFtemp[3]) + L3 * Math.Cos(jointAngle4DOFtemp[1]) * Math.Sin(jointAngle4DOFtemp[3]);
                    n = L2 * Math.Cos(jointAngle4DOFtemp[1]) + L3 * Math.Cos(jointAngle4DOFtemp[1]) * Math.Cos(jointAngle4DOFtemp[3]) - L3 * Math.Sin(jointAngle4DOFtemp[1]) * Math.Sin(jointAngle4DOFtemp[3]);
                    t = a - L1 * Math.Sin(jointAngle4DOFtemp[1]);
                    p = Math.Pow(Math.Pow(n, 2) + Math.Pow(m, 2), 0.5);
                    q = ANG2RAD(180) - Math.Asin(m / p);
                    jointAngle4DOFtemp[2] = -(Math.Asin(t / p) - q);
                    if (Math.Abs(InverseKinematics.RAD2ANG(jointAngle4DOFtemp[2])) >= 135) { jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]); continue; }
                    x1 = (L1 * Math.Sin(jointAngle4DOFtemp[1]) + L2 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3])) * Math.Cos(jointAngle4DOFtemp[0]);
                    y1 = (L1 * Math.Sin(jointAngle4DOFtemp[1]) + L2 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Sin(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3])) * Math.Sin(jointAngle4DOFtemp[0]);
                    z1 = L1 * Math.Cos(jointAngle4DOFtemp[1]) + L2 * Math.Cos(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2]) + L3 * Math.Cos(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3]);

                    jointAngle4DOFtemp[1] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[1]);
                    jointAngle4DOFtemp[2] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[2]);
                    jointAngle4DOFtemp[3] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[3]);
                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 3 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 6 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                    if (Math.Abs(jointAngle4DOFtemp[1] + jointAngle4DOFtemp[2] + jointAngle4DOFtemp[3] - 90) < 10 && x1 < (x + 0.5) && x1 > (x - 0.5) && y1 < (y + 0.5) && y1 > (y - 0.5) && z1 < (z + 0.5) && z1 > (z - 0.5))
                    {
                        ifsol = true;
                        jointAngle4DOF[1] = jointAngle4DOFtemp[1];
                        jointAngle4DOF[2] = jointAngle4DOFtemp[2];
                        jointAngle4DOF[3] = jointAngle4DOFtemp[3];
                        break;

                    }

                }
                else
                {
                    jointAngle4DOFtemp[1] = RAD2ANG(jointAngle4DOFtemp[1]);

                }

            }

            jointAngle4DOF[0] = InverseKinematics.RAD2ANG(jointAngle4DOFtemp[0]);
            return ifsol;
        }

        public static bool IK3DOF(double x, double y, double z, out double[] joint)   //机械臂逆解
        {
            joint = new double[3];
            bool hassol = false;
            double a, b,yy=0;
            double L1 = 15, L2 = 16,L3=5.2;
            double j1, j2, j0;
            double x1, y1, z1;
            if (y < 0)
            {
                yy = y;
                y = -y;
            }
            else
            {
                yy = y;
            }
            j0 = Math.Atan2(y, x);
            a = x / Math.Cos(j0);
            if (x == 0) a = y; //如果x为0，需要交换x，y
            b = z;
            j2 = Math.Acos((Math.Pow(a- L3, 2) + Math.Pow(b+5, 2) - Math.Pow(L1, 2) - Math.Pow(L2, 2)) / (2 * L2 * L1));

            for (j1 = -90; j1 < 90; j1 += 0.1)//找精度为0.1的解
                {
                    j1 = ANG2RAD(j1);
                    x1 = ((L1 * Math.Sin(j1) + L2 * Math.Sin(j1 + j2)) + L3) * Math.Cos(j0);
                    y1 = ((L1 * Math.Sin(j1) + L2 * Math.Sin(j1 + j2)) + L3) * Math.Sin(j0);
                    z1 = L1 * Math.Cos(j1) + L2 * Math.Cos(j1 + j2) - 5;
                    if (x1 < (x + 0.1) && x1 > (x - 0.1) && y1 < (y + 0.1) && y1 > (y - 0.1) && z1 < (z + 0.1) && z1 > (z - 0.1))
                    {
                    if (yy < 0)
                    {
                        j0 = -j0;

                    }
                    joint[0] = RAD2ANG(j0);
                        joint[1] = RAD2ANG(j1);
                        joint[2] = RAD2ANG(j2) + joint[1] - 90;
                        hassol = true;
                        break;

                    }
                    j1 = RAD2ANG(j1);//不换回去程序无法继续执行
                }
            if (!hassol)for (j1 = -90; j1 < 90; j1 += 0.1)//找精度为0.3的解
                {
                j1 = ANG2RAD(j1);
                x1 = ((L1 * Math.Sin(j1) + L2 * Math.Sin(j1 + j2))+ L3) * Math.Cos(j0);
                y1 = ((L1 * Math.Sin(j1) + L2 * Math.Sin(j1 + j2))+ L3) * Math.Sin(j0);
                z1 = L1 * Math.Cos(j1) + L2 * Math.Cos(j1 + j2) - 5;
                if (x1 < (x + 0.3) && x1 > (x - 0.3) && y1 < (y + 0.3) && y1 > (y - 0.3) && z1 < (z + 0.3) && z1 > (z - 0.3))
                {
                        if (yy < 0)
                        {
                            j0 = -j0;

                        }
                        joint[0] = RAD2ANG(j0);
                    joint[1] = RAD2ANG(j1);
                    joint[2] = RAD2ANG(j2) + joint[1] - 90 ;
                    hassol = true;
                    break;

                }
                j1 = RAD2ANG(j1);//不换回去程序无法继续执行
            }

            if (!hassol) for (j1 = -90; j1 < 90; j1 += 0.1)//找精度为0.6的解
                {
                    j1 = ANG2RAD(j1);
                    x1 = ((L1 * Math.Sin(j1) + L2 * Math.Sin(j1 + j2)) + L3) * Math.Cos(j0);
                    y1 = ((L1 * Math.Sin(j1) + L2 * Math.Sin(j1 + j2)) + L3) * Math.Sin(j0);
                    z1 = L1 * Math.Cos(j1) + L2 * Math.Cos(j1 + j2) - 5;
                    if (x1 < (x + 0.6) && x1 > (x - 0.6) && y1 < (y + 0.6) && y1 > (y - 0.6) && z1 < (z + 0.6) && z1 > (z - 0.6))
                    {
                        if (yy < 0)
                        {
                            j0 = -j0;

                        }
                        joint[0] = RAD2ANG(j0);
                        joint[1] = RAD2ANG(j1);
                        joint[2] = RAD2ANG(j2) + joint[1] - 90;
                        hassol = true;
                        break;

                    }
                    j1 = RAD2ANG(j1);//不换回去程序无法继续执行
                }
            if (!hassol)
            {
                joint[0] = 0;
                joint[1] = 0;
                joint[2] = 0;

            }
            return hassol;
        }

        public static void Transformation(double[] setoff, double offset_ang, double offset_x , double offset_y, out double[] newsetoff)
        {
            offset_ang = ANG2RAD(offset_ang);
            double[] B = new double[4];
            newsetoff = new double[3];
            Matrix matrix_before = Matrix.AddCol(new Matrix(setoff), new Matrix(1));
            Matrix matrix_later = new Matrix(B);

            double[,] t = new double[4, 4] { { Math.Cos(offset_ang), -Math.Sin(offset_ang)  ,0  ,   offset_x},
                                             { Math.Sin(offset_ang), Math.Cos(offset_ang) ,0  ,   offset_y},
                                             { 0 , 0     ,1  ,   0},
                                             { 0 , 0     ,0  ,   1}};

            Matrix T = new Matrix(t);
            matrix_later = Matrix.Reverse(T) * Matrix.Transfer(matrix_before);
            newsetoff[0] = matrix_later[0, 0];
            newsetoff[1] = matrix_later[1, 0];
            newsetoff[2] = matrix_later[2, 0];

        }
    }
}
