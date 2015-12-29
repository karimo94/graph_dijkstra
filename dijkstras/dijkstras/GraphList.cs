using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dijkstras
{
    class GraphList
    {
        class EdgeNode
        {
            public int adjGraphNode;
            public EdgeNode next;
        }
        class GraphNode
        {
            public bool visited;
            public string description;
            public EdgeNode head;
        }
        GraphNode[] adjList;
        int graphSize;
        public GraphList()
        {
        }
        public void initGraph(string textFile)
        {
            StreamReader sr = new StreamReader(textFile);
            graphSize = Convert.ToInt32(sr.ReadLine()) + 1;//no zero index used
            adjList = new GraphNode[graphSize];
            for (int i = 1; i < graphSize; i++)
            {
                adjList[i] = new GraphNode();
                adjList[i].head = new EdgeNode();
                adjList[i].head.adjGraphNode = i;//index of the edge node 
                adjList[i].head.next = null;
            }
            if (sr.EndOfStream)
            {
                return;
            }
            //read in descriptions
            for (int i = 1; i < graphSize; i++)
            {
                string[] line = sr.ReadLine().Split(',');
                adjList[i].description = line[0];
            }
            if (sr.EndOfStream)
            {
                return;
            }
            //read in distances
            while (!sr.EndOfStream)
            {
                int from, to;
                string[] line = sr.ReadLine().Split(',');
                from = Convert.ToInt32(line[0]);
                to = Convert.ToInt32(line[1]);
                insertEdge(from, to);
            }
        }
        public void dfs()
        {
            Console.WriteLine("***********DFS Ouput***********");
            Console.Write("Order: ");
            for (int v = 1; v < graphSize; v++)
            {
                if (!adjList[v].visited)
                {
                    dfsRecursive(v);
                }
            }
            Console.WriteLine();
        }
        public void displayAdjList()
        {
            Console.WriteLine();
            Console.WriteLine("***********Adjacency List***********");
            EdgeNode current = null;
            EdgeNode nextInList = null;
            for(int i = 1; i < graphSize; i++)
            {
                current = adjList[i].head;
                if (current.next != null)
                {
                    nextInList = current.next;
                }
                Console.WriteLine("Node: {0}, Description:{1}", current.adjGraphNode, adjList[i].description);

                while (current != null && nextInList != null)
                {
                    Console.Write(current.adjGraphNode);
                    while (nextInList != null)
                    {
                        string line = "->" + nextInList.adjGraphNode;
                        Console.Write(line);
                        nextInList = nextInList.next;
                    }
                }
                Console.WriteLine();
            }
        }
        private void dfsRecursive(int vertex)
        {
            adjList[vertex].visited = true;
            Console.Write("{0} ", vertex);
            int w = vertex;
            EdgeNode n = null;
            if (adjList[vertex].head.next != null)
            {
                n = adjList[vertex].head.next;
                w = n.adjGraphNode;
            }
            if (!adjList[w].visited)
            {
                dfsRecursive(w);
            }
        }
        private void insertEdge(int fromNode, int toNode)
        {
            EdgeNode n = new EdgeNode();
            n.adjGraphNode = toNode;
            n.next = null;
            n.next = adjList[fromNode].head.next;
            adjList[fromNode].head.next = n;
        }
    }
}
