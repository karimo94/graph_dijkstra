using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dijkstras
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph gr = new Graph();
            GraphList gl = new GraphList();
            gr.Display();
            gr.RunDijkstra();
            gl.initGraph("graph.txt");
            gl.displayAdjList();
            gl.dfs();
            Console.ReadLine();
        }
    }
}