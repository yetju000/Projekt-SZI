using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;

public class ID3 : MonoBehaviour
{

    public class Plant
    {

        public string name { get; set; }
        public string height { get; set; }
        public string color { get; set; }
        public string isSick { get; set; }
        public string lacksWater { get; set; }
        public bool harvest { get; set; }

        public Plant(string n, string h, string c, string iss, string lw, bool har)
        {
            this.name = n;
            this.height = h;
            this.color = c;
            this.isSick = iss;
            this.lacksWater = lw;
            this.harvest = har;
        }
    }

    public class TreeNode<T>
    {
        // wartosc przechowywana w nodzie
        private T value;

        // czy ten node ma parenta czy nie
        private bool hasParent;

        //lista potomkow node'a, zero lub wiecej
        private List<TreeNode<T>> children;

        // konstuuje node'a
        // value: value node'a
        public TreeNode(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Nie mozna umiescic pustej wartosci");
            }
            this.value = value;
            this.children = new List<TreeNode<T>>();
        }


        // value node'a
        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        //liczba potomkow danego node'a
        public int ChildrenCount
        {
            get { return this.children.Count; }
        }


        //dodaj potomka do node'a
        public void AddChild(TreeNode<T> child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("Nie mozna umiescic pustej wartosci");
            }
            if (child.hasParent)
            {
                throw new ArgumentException("Ta galaz juz ma rodzica");
            }

            child.hasParent = true;
            this.children.Add(child);
        }

        // zwraca i-tego potomka
        public TreeNode<T> GetChild(int index)
        {
            return this.children[index];
        }

    }

    // DRZEWO
    public class Tree<T>
    {
        private KeyValuePair<string, double> kvp;

        // buduje drzewo z korzeniem - root i wartoscia value
        public Tree(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Cannot insert null value");
            }
            this.Root = new TreeNode<T>(value);
        }


        // buduje drzewo z potomkami podanymi jako parametr
        public Tree(T value, params Tree<T>[] children) : this(value)
        {
            foreach (Tree<T> child in children)
            {
                this.Root.AddChild(child.Root);
            }
        }

        // zwraca korzen albo null jak drzewo puste
        public TreeNode<T> Root { get; }

        // wypisuje drzewko DFS - po korzeniach
        private void PrintDFS(TreeNode<T> root, string spaces)
        {
            if (this.Root == null)
                return;

            Console.WriteLine(spaces + root.Value);

            TreeNode<T> child = null;
            for (int i = 0; i < root.ChildrenCount; i++)
            {
                child = root.GetChild(i);
                PrintDFS(child, spaces + "\t -->");
            }

        }

        // odwiedza i rysuje drzewko DFS
        public void TraverseDFS()
        {
            this.PrintDFS(this.Root, string.Empty);
        }
    }

    public List<string> header = new List<string>();





    //private static System.Random rnd;

    // Use this for initialization
    void Start()
    {

        Dictionary<string, double> en = new Dictionary<string, double>();
        Dictionary<string, double> ig = new Dictionary<string, double>();
        List<KeyValuePair<string, double>> ig2 = new List<KeyValuePair<string, double>>();

        List<Plant> rosliny = new List<Plant>();
    }

    private static System.Random rnd;

    header.Add("name");
        header.Add("height");
        header.Add("color");
        header.Add("isSick");
        header.Add("lacksWater");

        double numPlants = rosliny.Count;
    double eps = Double.Epsilon;

    // generuje losowo roślinki
    string[] names = { "corn", "wheat", "colza", "tulip" };

    string[] colorsCorn = { "yellow", "brown" };
    string[] colorsWheat = { "yellow", "green" };
    string[] colorTulip = { "purple", "green" };
    string[] trueFalseString = { "true", "false" };
    string[] heightTulip = { "50", "60", "70", "80", "90" };
    string[] heightCorn = { "150", "160", "170", "180", "190" };
    string[] heightColza = { "120", "130", "140", "150", "160" };
    string[] heightWheat = { "110", "120", "130", "140", "150" };
    bool[] trueFalse = { true, false };

    string[] allValues =
    {
            "50", "60", "70", "80", "90",
                "110", "120", "130", "140", "150",
                "160", "170", "180", "190",
                "yellow", "brown",
                "purple", "green",
                "true", "false"
        };

    string[] allHeightAttributes =
    {
            "50", "60", "70", "80", "90",
                "110", "120", "130", "140", "150",
                "160", "170", "180", "190",
        };
    string[] allColorAttributes =
    {
            "yellow", "brown", "purple", "green"
        };
}

void generatePlant()
{
    string newName;
    string newHeight;
    string newColor;
    string newIsSick;
    string newLacksWater;
    bool newHarvest;

    newHarvest = false;
    rnd = new Random();
    newName = names[rnd.Next(names.Length)];

    if (newName == "corn")
    {
        rnd = new Random();
        newHeight = heightCorn[rnd.Next(heightCorn.Length)];
        newColor = colorsCorn[rnd.Next(colorsCorn.Length)];
        newIsSick = trueFalseString[rnd.Next(trueFalseString.Length)];
        newLacksWater = trueFalseString[rnd.Next(trueFalseString.Length)];
        if (newHeight == "150" || newIsSick == "true") newHarvest = false;
        else newHarvest = true;


        var p = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
        rosliny.Add(p);
    }
    else if (newName == "wheat")
    {
        rnd = new Random();
        newHeight = heightWheat[rnd.Next(heightWheat.Length)];
        newColor = colorsWheat[rnd.Next(colorsWheat.Length)];
        newIsSick = trueFalseString[rnd.Next(trueFalseString.Length)];
        newLacksWater = trueFalseString[rnd.Next(trueFalseString.Length)];
        if (newColor == "green" || newIsSick == "true") newHarvest = false;
        else newHarvest = true;

        var p = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
        rosliny.Add(p);
    }
    else if (newName == "colza")
    {
        rnd = new Random();
        newHeight = heightColza[rnd.Next(heightColza.Length)];
        newColor = colorsWheat[rnd.Next(colorsWheat.Length)];
        newIsSick = trueFalseString[rnd.Next(trueFalseString.Length)];
        newLacksWater = trueFalseString[rnd.Next(trueFalseString.Length)];
        //newHarvest = trueFalse[rnd.Next(trueFalse.Length)];
        if (newLacksWater == "true" || newIsSick == "true") newHarvest = false;
        else newHarvest = true;

        var p = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
        rosliny.Add(p);
    }
    else if (newName == "tulip")
    {
        rnd = new Random();
        newHeight = heightTulip[rnd.Next(heightTulip.Length)];
        newColor = colorsWheat[rnd.Next(colorsWheat.Length)];
        newIsSick = trueFalseString[rnd.Next(trueFalseString.Length)];
        newLacksWater = trueFalseString[rnd.Next(trueFalseString.Length)];
        //newHarvest = trueFalse[rnd.Next(trueFalse.Length)];
        if (newHeight == "50" || newIsSick == "true") newHarvest = false;
        else newHarvest = true;

        var p = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
        rosliny.Add(p);
    }

}


}


    // Update is called once per frame
    void Update()
{

}
}
