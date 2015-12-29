using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dijkstras
{
    class Graph
    {
        class NodeData
        {
            private int index;
            public string data;
            public NodeData(string data, int index)
            {
                this.index = index;
                this.data = data;
            }
        }
        /// <summary>
        /// 4 attributes
        /// A list of vertices (to store node information for each index such as name/text)
        /// a 2D array - our adjacency matrix, stores edges between vertices
        /// a graphSize integer
        /// a StreamReader, to read in graph data to create the data structure
        /// </summary>
        private List<NodeData> vertices;
        private int graphSize;
        private StreamReader sr;
        private int[,] adjMatrix;
        private const int infinity = 9999;
        public Graph()
        {
            vertices = new List<NodeData>();
            sr = new StreamReader("graph2.txt");

            CreateGraph();
        }
        private void CreateGraph()
        {
            //get the graph size first
            graphSize = Convert.ToInt32(sr.ReadLine()) + 1;//non-zero arrays, add 1
            adjMatrix = new int[graphSize, graphSize];
            for (int i = 0; i < graphSize; i++)
            {
                adjMatrix[i, 0] = i;
            }
            if (sr.EndOfStream)//check if we're done
            {
                return;
            }
            //init vertices
            vertices.Add(new NodeData("    ", -1));//no zero index to be used
            for (int i = 1; i < graphSize; i++)
            {
                string[] line = sr.ReadLine().Split(',');
                vertices.Add(new NodeData(line[0], Convert.ToInt32(line[1])));
            }
            if (sr.EndOfStream)//check if we're done
            {
                return;
            }
            //initialize the adjacency matrix
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(',');
                int fromNode = Convert.ToInt32(line[0]);
                int toNode = Convert.ToInt32(line[1]);
                int cost = Convert.ToInt32(line[2]);
                AddEdge(fromNode, toNode, cost);
            }
        }
        public void RunDijkstra()//runs dijkstras algorithm on the adjacency matrix
        {
            Console.WriteLine("***********Dijkstra's Shortest Path***********");
            int[] distance = new int[graphSize];
            int[] previous = new int[graphSize];

            for (int source = 1; source < graphSize; source++)
            {
                for (int i = 1; i < graphSize; i++)
                {
                    distance[i] = infinity;
                    previous[i] = 0;
                }
                distance[source] = 0;
                PriorityQueue<int> pq = new PriorityQueue<int>();
                //enqueue the source
                pq.Enqueue(source, adjMatrix[source, source]);
                //insert all remaining vertices into the pq, could also be find v...
                for (int i = 1; i < graphSize; i++)
                {
                    for (int j = 1; j < graphSize; j++)
                    {
                        if (adjMatrix[i, j] > 0 && adjMatrix[i,j] != adjMatrix[source,source])
                        {
                            pq.Enqueue(i, adjMatrix[i, j]);
                        }
                    }
                }
                while (!pq.Empty())
                {
                    int u = pq.dequeue_min();

                    for (int v = 1; v < graphSize; v++)//scan each row fully
                    {
                        if (adjMatrix[u, v] > 0)//if there is an adjacent node
                        {
                            int alt = distance[u] + adjMatrix[u, v];
                            if (alt < distance[v])
                            {
                                distance[v] = alt;
                                previous[v] = u;
                                pq.Enqueue(u, distance[v]);
                            }
                        }
                    }
                }
                //distance to 1..2..3..4..5..6 etc lie inside each index

                for (int i = 1; i < graphSize; i++)
                {
                    if (distance[i] == infinity || distance[i] == 0)
                    {
                        Console.WriteLine("Distance from {0} to {1}: --", source, i);
                    }
                    else
                    {
                        Console.WriteLine("Distance from {0} to {1}: {2}", source, i, distance[i]);
                    }
                }
                for (int i = 1; i < graphSize; i++)
                {
                    printPath(previous, source, i);
                    Console.WriteLine();
                } 
                Console.WriteLine();
            }
        }
        private void printPath(int[] path, int start, int end)
        {
            //prints a path, given a start and end, and an array that holds previous 
            //nodes visited
            Console.WriteLine("Shortest path from {0} to {1}:", start, end);
            int temp = end;
            Stack<int> s = new Stack<int>();
            while (temp != start)
            {
                s.Push(temp);
                temp = path[temp];
                if (temp == 0)
                {
                    break;
                }
            }
            if (temp == 0 || start == end)
            {
                Console.Write("No path");
                s.Clear();
                return;
            }
            Console.Write("{0} ", temp);//print source
            while (s.Count != 0)
            {
                Console.Write("{0} ", s.Pop());//print successive nodes to destination
            }
        }
        public void AddEdge(int vertexA, int vertexB, int distance)
        {
            if (vertexA > 0 && vertexB > 0 && vertexA <= graphSize && vertexB <= graphSize)
            {
                adjMatrix[vertexA, vertexB] = distance;
            }
        }
        public void RemoveEdge(int vertexA, int vertexB)
        {
            if (vertexA > 0 && vertexB > 0 && vertexA <= graphSize && vertexB <= graphSize)
            {
                adjMatrix[vertexA, vertexB] = 0;
            }
        }
        public bool Adjacent(int vertexA, int vertexB)
        {   //checks whether two vertices are adjacent, returns true or false
            return (adjMatrix[vertexA, vertexB] > 0);
        }
        public int length(int vertex_u, int vertex_v)//returns a distance between 2 nodes
        {
            return adjMatrix[vertex_u, vertex_v];
        }
        public void Display() //displays the adjacency matrix
        {
            Console.WriteLine("***********Adjacency Matrix Representation***********");
            Console.WriteLine("Number of nodes: {0}\n", graphSize - 1);
            foreach (NodeData n in vertices)
            {
                Console.Write("{0}\t", n.data);
            }
            Console.WriteLine();//newline for the graph display
            for (int i = 1; i < graphSize; i++)
            {
                Console.Write("{0}\t", vertices[adjMatrix[i,0]].data);
                for (int j = 1; j < graphSize; j++)
                {
                    Console.Write("{0}\t", adjMatrix[i,j]);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine("Read the graph from left to right");
            Console.WriteLine("Example: Node A has an edge to Node C with distance: {0}",
                length(1,3));
        }
        private void DisplayNodeData(int v)//displays data/description for a node
        {
            Console.WriteLine(vertices[v].data);
        }
    }
}
