using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nodes
{
    public partial class Form1 : Form
    {
        public List<Node> nodesArr = new List<Node>
            {
                new Node(1, 10,50),
                new Node(2, 110,50),
                new Node(3, 210,50),
                new Node(4, 60,100),
                new Node(5, 160,100),
                new Node(6, 10,150),
                new Node(7, 110,150),
                new Node(8, 210,150),
                new Node(9, 60,200),
                new Node(10, 160,200),
                new Node(11, 10,250),
                new Node(12, 110,250),
                new Node(13, 210,250),
                new Node(14, 60,300),
                new Node(15, 160,300)
            };

        private Node startNode = new Node();
        private Node desiredNode = new Node();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startNode = nodesArr.FirstOrDefault();
            desiredNode = nodesArr.LastOrDefault();

        }

        public void ButtonClick(object sender, EventArgs e)
        {
            Control btn = (Control)sender;

            if (btn.BackColor == Color.Red)
            {
                btn.BackColor = Color.Green;
                nodesArr.FirstOrDefault(x => x.Id.ToString() == btn.Text).Enabled = true;
            }
            else
            {
                btn.BackColor = Color.Red;
                nodesArr.FirstOrDefault(x => x.Id.ToString() == btn.Text).Enabled = false;
            }

            Invalidate();
            Update();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Node n in nodesArr)
            {
                Button btn = new Button()
                {
                    Location = new Point(n.X, n.Y),
                    Text = n.Id.ToString(),
                    Size = new Size(30, 30),
                    BackColor = Color.Green
                };

                btn.Click += new EventHandler(ButtonClick);
                Controls.Add(btn);

                listBox1.Items.Add(n.Id);
                listBox2.Items.Add(n.Id);
            }

            listBox1.SelectedItem = startNode.Id;
            listBox2.SelectedItem = desiredNode.Id;

            List<Node> shortestPath = FindShortestPath(startNode, desiredNode, nodesArr.Where(x => x.Enabled).ToList());

            Node lastNode = null;
            foreach (Node node in shortestPath)
            {
                if (lastNode != null)
                {
                    Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
                    e.Graphics.DrawLine(pen, lastNode.X + 15, lastNode.Y + 15, node.X + 15, node.Y + 15);
                }

                lastNode = node;
            }
        }

        public static double Distance(Node node1, Node node2)
        {
            int dx = node1.X - node2.X;
            int dy = node1.Y - node2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static List<Node> FindShortestPath(Node startNode, Node desiredNode, List<Node> nodes)
        {
            Dictionary<Node, Node> parentMap = new Dictionary<Node, Node>();
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(startNode);
            parentMap[startNode] = null;

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                if (currentNode == desiredNode)
                    break;

                foreach (Node neighbor in nodes)
                {
                    if (neighbor != currentNode && Distance(currentNode, neighbor) <= 99 && !parentMap.ContainsKey(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        parentMap[neighbor] = currentNode;
                    }
                }
            }

            // Reconstruct the path from startNode to desiredNode
            List<Node> shortestPath = new List<Node>();
            Node node = desiredNode;
            while (node != null)
            {
                shortestPath.Insert(0, node);
                if (parentMap.ContainsKey(node))
                {
                    node = parentMap[node];
                }
                else
                {
                    break;
                }
            }

            return shortestPath;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            startNode = nodesArr.FirstOrDefault(x => x.Id.ToString() == listBox.SelectedItem.ToString());

            Invalidate();
            Update();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            desiredNode = nodesArr.FirstOrDefault(x => x.Id.ToString() == listBox.SelectedItem.ToString());

            Invalidate();
            Update();
        }
    }
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Id { get; set; }
        public bool Enabled { get; set; }

        public Node()
        {
        }

        public Node(int id, int x, int y)
        {
            X = x;
            Y = y;
            Id = id;
            Enabled = true;
        }
    }
}
