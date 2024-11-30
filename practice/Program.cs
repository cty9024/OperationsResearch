using System;
using System.IO;
using System.Collections.Generic;
using ILOG.CPLEX;
using ILOG.Concert;

namespace practice
{
    class Program
    {
        static void Main(string[] args)
        {
            Cplex cplex = new Cplex();

            StreamReader Set_d = new StreamReader(@"../../../Demand.csv");

            List<string> D = new List<string>();
            string Set_line;
            Set_line = Set_d.ReadLine();
            while ((Set_line = Set_d.ReadLine()) != null)
            {
                for (int i = 0; i < 24; i++)
                {
                    D.Add(Set_line.Split(',')[2 + i]);
                }
            }
            Set_d.Close();

            StreamReader Set_s = new StreamReader(@"../../../Sales.csv");

            List<string> S = new List<string>();
            Set_line = Set_s.ReadLine();
            while ((Set_line = Set_s.ReadLine()) != null)
            {
                S.Add(Set_line.Split(',')[2]);
            }
            Set_s.Close();

            StreamReader Set_c = new StreamReader(@"../../../Capacity.csv");

            List<string> C = new List<string>();
            Set_line = Set_c.ReadLine();
            Set_line = Set_c.ReadLine();
            foreach (string a in Set_line.Split(','))
            {
                C.Add(a);
            }


            Set_c.Close();

            foreach (string a in C)
            {
                Console.WriteLine(a);
            }

            //變數
            INumVar[][][] Make = new INumVar[C.Count][][];
            for (int i = 0; i < C.Count; i++)
            {
                Make[i] = new INumVar[S.Count][];
                for (int j = 0; j < S.Count; j++)
                {
                    Make[i][j] = new INumVar[24];
                    
                    for(int k=0; k<24; k++)
                    {
                        Make[i][j][k] = cplex.NumVar(0, 1E50);
                    } 
                }
            }

            INumVar[][] kk = new INumVar[S.Count][];
            for (int i = 0; i < S.Count; i++)
            {
                kk[i] = new INumVar[24];
                for(int j=0; j<24; j++)
                {
                    kk[i][j] = cplex.NumVar(0, 1E50);
                }
            }

            ILinearNumExpr[][] bb = new ILinearNumExpr[S.Count][];
            for (int i = 0; i < S.Count; i++)
            {
                bb[i] = new ILinearNumExpr[24];
                for(int j=0; j<24; j++)
                {
                    bb[i][j] = cplex.LinearNumExpr();
                }
            }

            INumExpr[][] Inventory = new INumExpr[S.Count][];
            for (int i = 0; i < S.Count; i++)
            {
                Inventory[i] = new INumExpr[24];
                for(int j=0; j<24; j++)
                {
                    Inventory[i][j] = cplex.NumExpr();
                }
            }

            //目標式
            ILinearNumExpr eq = cplex.LinearNumExpr();
            double temp;
            
            for (int i=0;i<S.Count;i++)
            {
                double.TryParse(S[i].ToString(), out temp);
                for (int j=0;j<24;j++)
                {
                    eq.AddTerm(temp, kk[i][j]);
                }
            }
            

            for (int i = 0; i < S.Count; i++)
            {
                double.TryParse(S[i].ToString(), out temp);
                for (int j = 0; j < C.Count; j++)
                {
                    for (int k=0;k<24;k++)
                    {
                        eq.AddTerm(-0.5 * temp, Make[j][i][k]);
                    }                    
                }
            }

            //ILinearNumExpr eq = cplex.LinearNumExpr();
            INumExpr eqqq = cplex.NumExpr();
            INumExpr[] iTmp = new INumExpr[S.Count];
            for (int i = 0; i < S.Count; i++)
            {
                iTmp[i] = cplex.Sum(Inventory[i]);
            }
            eqqq = cplex.Prod(-1, cplex.Sum(iTmp));

            INumExpr eqq = cplex.NumExpr();
            INumExpr[] bTmp = new INumExpr[S.Count];
            for (int i = 0; i < S.Count; i++)
            {
                double.TryParse(S[i].ToString(), out temp);                
                bTmp[i] = cplex.Prod(temp, cplex.Sum(bb[i]));                
            }
            eqq = cplex.Prod(-2, cplex.Sum(bTmp));


            INumExpr t = cplex.Sum(eq, eqqq, eqq);
            cplex.AddMaximize(t);
            
            //限制式
            ILinearNumExpr[][] avai = new ILinearNumExpr[C.Count][];
            for (int i = 0; i < C.Count; i++)
            {
                avai[i] = new ILinearNumExpr[24];
               for(int j=0; j<24; j++)
                {
                    avai[i][j] = cplex.LinearNumExpr();
                }
            }

            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < S.Count; j++)
                {
                    for (int k = 0; k < C.Count; k++)
                    {
                        avai[k][i].AddTerm(1.0, Make[k][j][i]);
                    }
                }

                //double temp;
                for (int j = 0; j < C.Count; j++)
                {
                    double.TryParse(C[j].ToString(), out temp);
                    cplex.AddLe(avai[j][i], temp);
                }
            }

            ILinearNumExpr[][] num_s = new ILinearNumExpr[S.Count][];
            for (int i = 0; i < S.Count; i++)
            {
                num_s[i] = new ILinearNumExpr[24];
            }

            ILinearNumExpr[][] Kleft = new ILinearNumExpr[S.Count][];
            for (int i = 0; i < S.Count; i++)
            {
                Kleft[i] = new ILinearNumExpr[24];
                for(int j=0; j<24; j++)
                {
                    Kleft[i][j] = cplex.LinearNumExpr();
                }
            }
            INumExpr[][] Kright = new INumExpr[S.Count][];
            for (int i = 0; i < S.Count; i++)
            {
                Kright[i] = new INumExpr[24];
                for(int j=0; j<24; j++)
                {
                    Kright[i][j] = cplex.NumExpr();
                 }
            }


            ILinearNumExpr[][] xMake = new ILinearNumExpr[S.Count][];
            for (int i = 0; i < S.Count; i++)
            {
                xMake[i] = new ILinearNumExpr[24];
                for(int k=0; k<24; k++)
                {
                    xMake[i][k] = cplex.LinearNumExpr();
                }
                for (int j = 0; j < 24; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        xMake[i][j].AddTerm(1.0, Make[k][i][j]);
                    }
                    if (j == 0)
                    {
                        cplex.AddEq(0, bb[i][j]);
                    }
                    else
                    {
                        double.TryParse(D[24*i+j].ToString(), out temp);
                        cplex.AddEq(bb[i][j], cplex.Sum(temp, cplex.Prod(-1.0, xMake[i][j])));
                    }                    
                }
            }

            for (int i = 0; i < S.Count; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    if (j == 0)
                    {
                        //Inventory[i][j] = cplex.Sum(xMake[i][j], cplex.Prod(-1.0, demand[i][j]), bb[i][j]);
                        cplex.AddEq(0, Inventory[i][0]);
                    }

                    else
                    {
                        double.TryParse(D[24 * i + j].ToString(), out temp);
                        cplex.AddEq(Inventory[i][j], cplex.Sum(-1.0 * temp, cplex.Sum(Inventory[i][j - 1], xMake[i][j], cplex.Prod(-1.0, bb[i][j - 1]), bb[i][j])));
                    }
                }
            }

            for (int i = 0; i < S.Count; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    Kright[i][j] = cplex.Sum(xMake[i][j], Inventory[i][j]);
                    Kleft[i][j].AddTerm(1.0, kk[i][j]);
                }

                for (int j = 0; j < 24; j++)
                {
                    cplex.AddLe(Kleft[i][j], Kright[i][j]);
                }
            }
            
            
            cplex.Solve();
            cplex.ExportModel("test.lp");
            Console.WriteLine(cplex.GetObjValue());
        }
    }
}
