﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace id3
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



    class Program
    {
        private static Random rnd;

        static void Main(string[] args)
        {

            Dictionary<string, double> en = new Dictionary<string, double>();
            Dictionary<string, double> ig = new Dictionary<string, double>();
            List<KeyValuePair<string, double>> ig2 = new List<KeyValuePair<string, double>>();

            //Dictionary<string, string[]> nodes = new Dictionary<string, string[]>();
            //List<ClassNode> classNodes = new List<ClassNode>();


            List<Plant> rosliny = new List<Plant>();

            double numPlants = rosliny.Count;
            double eps = Double.Epsilon;

            // przynajmniej 1 przypadek true z kazdej rosliny
            string newName;
            string newHeight;
            string newColor;
            string newIsSick;
            string newLacksWater;
            bool newHarvest;
            // corn
            newName = "corn";
            newHeight = "150";
            newColor = "yellow";
            newIsSick = "false";
            newLacksWater = "false";
            newHarvest = true;
            var p1 = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
            rosliny.Add(p1);
            // wheat
            newName = "wheat";
            newHeight = "140";
            newColor = "yellow";
            newIsSick = "false";
            newLacksWater = "false";
            newHarvest = true;
            var p2 = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
            rosliny.Add(p2);
            // colza
            newName = "colza";
            newHeight = "130";
            newColor = "yellow";
            newIsSick = "false";
            newLacksWater = "false";
            newHarvest = true;
            var p3 = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
            rosliny.Add(p3);
            // tulip
            newName = "tulip";
            newHeight = "80";
            newColor = "purple";
            newIsSick = "false";
            newLacksWater = "false";
            newHarvest = true;
            var p4 = new Plant(newName, newHeight, newColor, newIsSick, newLacksWater, newHarvest);
            rosliny.Add(p4);


            // generuje losowo roślinki

            string[] names = { "corn", "wheat", "colza", "tulip" };
            List<string> header = new List<string>();
            header.Add("name");
            header.Add("height");
            header.Add("color");
            header.Add("isSick");
            header.Add("lacksWater");
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

            void generatePlant()
            {
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

            // geneujemy rosliny
            void generatePlants(int n)
            {
                for (int i = 0; i < n; i++)
                {
                    generatePlant();
                    Thread.Sleep(20); // zeby wygenerowac randoma musze usypiac wątek, nice...
                }
            }
            generatePlants(1000);








            //////////////////
            //entropia i IG //
            //////////////////


            ///////////////////////////////////
            // licze entropie zestawu danych //
            ///////////////////////////////////

            double countMainEntropy(List<Plant> list)
            {
                double entropy = 0.0;
                double sumExamples = 0.0;

                var queryTrue = from p in rosliny where p.harvest == true select p; // harvest == "Yes"
                var queryFalse = from p in rosliny where p.harvest == false select p; // harvest == "No"

                double attPositive = queryTrue.Count();
                double attFalse = queryFalse.Count();
                sumExamples = attPositive + attFalse;

                double fract01 = attPositive / sumExamples; // pozytywne / wszystkie
                double fract02 = attFalse / sumExamples; // negatywne / wszystkie

                entropy = -fract01 * Math.Log(fract01, 2.0) - fract02 * Math.Log(fract02, 2.0);
                Console.WriteLine("Entropia klasy decyzyjnej: \t " + entropy);

                return entropy;
            }
            double entropyMain = countMainEntropy(rosliny);




            ///////////////////////////////////////////////////////////////
            // Entropia i Ig dla wszystkich klas, celem wyłonienia roota //
            ///////////////////////////////////////////////////////////////
            double entropyForClass(List<Plant> list, List<string> klasy)
            {
                double entropy = 0.0;
                double igValue = 0.0;
                double fract = 0.0;
                double numberPositive = 0.0;
                double numberNegative = 0.0;
                double sumExamples = 0.0;
                string[] valString;

                foreach (string klasa in klasy)
                {
                    if (klasa == "name")
                    {
                        fract = 0.0;
                        entropy = 0.0;
                        valString = rosliny.Select(o => o.name).Distinct().ToArray();
                        int counter = rosliny.Count();
                        foreach (var value in valString)
                        {
                            for (int i = 0; i < counter; i++)
                            {
                                if (rosliny[i].name == value && rosliny[i].harvest == true)
                                    ++numberPositive;
                                if (rosliny[i].name == value && rosliny[i].harvest == false)
                                    ++numberNegative;
                            }
                            sumExamples = numberPositive + numberNegative;

                            //jezeli tylko true albo false to zwracam liścia
                            if (numberPositive == 0)
                            {
                                return 0.0;
                            }
                            else if (numberNegative == 0)
                            {
                                return 1.0;
                            }


                            double fract01 = numberPositive / sumExamples + eps; // pozytywne / wszystkie
                            double fract02 = numberNegative / sumExamples + eps; // negatywne / wszystkie
                            double entropyAttribute = -fract01 * Math.Log(fract01, 2.0) - fract02 * Math.Log(fract02, 2.0);

                            //Console.WriteLine("entropia atrybutu "+ value +" " + entropyAttribute);
                            entropy += entropyAttribute * (sumExamples / counter);
                            // Console.WriteLine("Entropia klasy: " + h + " " + entropy);

                            numberPositive = 0.0;
                            numberNegative = 0.0;
                            entropyAttribute = 0.0;
                        }

                        Console.WriteLine("Entropia dla klasy " + klasa + ": \t" + entropy);
                        igValue = Math.Abs(entropyMain - entropy);
                        Console.WriteLine("IG dla klasy  " + klasa + ": \t" + igValue);

                        KeyValuePair<string, double> kvp = new KeyValuePair<string, double>(klasa, igValue);
                        ig2.Add(kvp);

                        // sprawdza, czy klucz i wartosc zostaly juz dodane wczesniej

                        if (!ig.ContainsValue(igValue))
                        {
                            ig.Add(klasa, igValue);
                        }

                        return igValue;

                    }

                    else if (klasa == "height")
                    {
                        fract = 0.0;
                        entropy = 0.0;
                        valString = rosliny.Select(o => o.height).Distinct().ToArray();
                        int counter = rosliny.Count();
                        foreach (var value in valString)
                        {
                            for (int i = 0; i < counter; i++)
                            {
                                if (rosliny[i].height == value && rosliny[i].harvest == true)
                                    ++numberPositive;
                                if (rosliny[i].height == value && rosliny[i].harvest == false)
                                    ++numberNegative;
                            }
                            sumExamples = numberPositive + numberNegative + eps;

                            //jezeli tylko true albo false to zwracam liścia - 0 - false, 1 - true
                            if (numberPositive == 0)
                            {
                                return 0.0;
                            }
                            else if (numberNegative == 0)
                            {
                                return 1.0;
                            }

                            double fract01 = numberPositive / sumExamples + eps; // pozytywne / wszystkie
                            double fract02 = numberNegative / sumExamples + eps; // negatywne / wszystkie
                            double entropyAttribute = -fract01 * Math.Log(fract01, 2.0) - fract02 * Math.Log(fract02, 2.0);

                            //Console.WriteLine("entropia atrybutu " + value + " " + entropyAttribute);
                            entropy += entropyAttribute * (sumExamples / counter);
                            //Console.WriteLine("Entropia klasy: " + h + " " + entropy);

                            numberPositive = 0.0;
                            numberNegative = 0.0;
                            entropyAttribute = 0.0;
                        }

                        Console.WriteLine("Entropia dla klasy " + klasa + ": \t" + entropy);
                        igValue = Math.Abs(entropyMain - entropy);
                        Console.WriteLine("IG dla klasy  " + klasa + ": \t" + igValue);

                        KeyValuePair<string, double> kvp = new KeyValuePair<string, double>(klasa, igValue);
                        ig2.Add(kvp);
                        if (!ig.ContainsValue(igValue))
                        {
                            ig.Add(klasa, igValue);
                        }

                        return igValue;

                    }

                    else if (klasa == "color")
                    {
                        fract = 0.0;
                        entropy = 0.0;
                        valString = rosliny.Select(o => o.color).Distinct().ToArray();
                        int counter = rosliny.Count();
                        foreach (var value in valString)
                        {
                            for (int i = 0; i < counter; i++)
                            {
                                if (rosliny[i].color == value && rosliny[i].harvest == true)
                                    ++numberPositive;
                                if (rosliny[i].color == value && rosliny[i].harvest == false)
                                    ++numberNegative;
                            }
                            sumExamples = numberPositive + numberNegative + eps;

                            //jezeli tylko true albo false to zwracam liścia - 0 - false, 1 - true
                            if (numberPositive == 0)
                            {
                                return 0.0;
                            }
                            else if (numberNegative == 0)
                            {
                                return 1.0;
                            }

                            double fract01 = numberPositive / sumExamples + eps; // pozytywne / wszystkie
                            double fract02 = numberNegative / sumExamples + eps; // negatywne / wszystkie
                            double entropyAttribute = -fract01 * Math.Log(fract01, 2.0) - fract02 * Math.Log(fract02, 2.0);

                            //Console.WriteLine("entropia atrybutu " + value + " " + entropyAttribute);
                            entropy += entropyAttribute * (sumExamples / counter);
                            //Console.WriteLine("Entropia klasy: " + h + " " + entropy);

                            numberPositive = 0.0;
                            numberNegative = 0.0;
                            entropyAttribute = 0.0;
                        }

                        Console.WriteLine("Entropia dla klasy " + klasa + ": \t" + entropy);
                        igValue = Math.Abs(entropyMain - entropy);
                        Console.WriteLine("IG dla klasy  " + klasa + ": \t" + igValue);

                        KeyValuePair<string, double> kvp = new KeyValuePair<string, double>(klasa, igValue);
                        ig2.Add(kvp);
                        if (!ig.ContainsValue(igValue))
                        {
                            ig.Add(klasa, igValue);
                        }
                        return igValue;

                    }

                    else if (klasa == "isSick")
                    {
                        fract = 0.0;
                        entropy = 0.0;
                        valString = rosliny.Select(o => o.isSick).Distinct().ToArray();
                        int counter = rosliny.Count();
                        foreach (var value in valString)
                        {
                            for (int i = 0; i < counter; i++)
                            {
                                if (rosliny[i].isSick == value && rosliny[i].harvest == true)
                                    ++numberPositive;
                                if (rosliny[i].isSick == value && rosliny[i].harvest == false)
                                    ++numberNegative;
                            }
                            sumExamples = numberPositive + numberNegative;

                            //jezeli tylko true albo false to zwracam liścia - 0 - false, 1 - true
                            if (numberPositive == 0)
                            {
                                return 0.0;
                            }
                            else if (numberNegative == 0)
                            {
                                return 1.0;
                            }

                            double fract01 = numberPositive / sumExamples + eps; // pozytywne / wszystkie
                            double fract02 = numberNegative / sumExamples + eps; // negatywne / wszystkie
                            double entropyAttribute = -fract01 * Math.Log(fract01, 2.0) - fract02 * Math.Log(fract02, 2.0);

                            //Console.WriteLine("entropia atrybutu " + value + " " + entropyAttribute);
                            entropy += entropyAttribute * (sumExamples / counter);
                            //Console.WriteLine("Entropia klasy: " + h + " " + entropy);

                            numberPositive = 0.0;
                            numberNegative = 0.0;
                            entropyAttribute = 0.0;
                        }

                        Console.WriteLine("Entropia dla klasy " + klasa + ": \t" + entropy);
                        igValue = Math.Abs(entropyMain - entropy);
                        Console.WriteLine("IG dla klasy  " + klasa + ": \t" + igValue);

                        KeyValuePair<string, double> kvp = new KeyValuePair<string, double>(klasa, igValue);
                        ig2.Add(kvp);
                        if (!ig.ContainsValue(igValue))
                        {
                            ig.Add(klasa, igValue);
                        }

                        return igValue;

                    }
                    else if (klasa == "lacksWater")
                    {
                        fract = 0.0;
                        entropy = 0.0;
                        valString = rosliny.Select(o => o.lacksWater).Distinct().ToArray();
                        int counter = rosliny.Count();
                        foreach (var value in valString)
                        {
                            for (int i = 0; i < counter; i++)
                            {
                                if (rosliny[i].lacksWater == value && rosliny[i].harvest == true)
                                    ++numberPositive;
                                if (rosliny[i].lacksWater == value && rosliny[i].harvest == false)
                                    ++numberNegative;
                            }
                            sumExamples = numberPositive + numberNegative + eps;

                            //jezeli tylko true albo false to zwracam liścia - 0 - false, 1 - true
                            if (numberPositive == 0)
                            {
                                return 0.0;
                            }
                            else if (numberNegative == 0)
                            {
                                return 1.0;
                            }

                            double fract01 = numberPositive / sumExamples + eps; // pozytywne / wszystkie
                            double fract02 = numberNegative / sumExamples + eps; // negatywne / wszystkie
                            double entropyAttribute = -fract01 * Math.Log(fract01, 2.0) - fract02 * Math.Log(fract02, 2.0);

                            //Console.WriteLine("entropia atrybutu " + value + " " + entropyAttribute);
                            entropy += entropyAttribute * (sumExamples / counter);
                            //Console.WriteLine("Entropia klasy: " + h + " " + entropy);

                            numberPositive = 0.0;
                            numberNegative = 0.0;
                            entropyAttribute = 0.0;
                        }

                        Console.WriteLine("Entropia dla klasy " + klasa + ": \t" + entropy);
                        igValue = Math.Abs(entropyMain - entropy);
                        Console.WriteLine("IG dla klasy  " + klasa + ": \t" + igValue);

                        KeyValuePair<string, double> kvp = new KeyValuePair<string, double>(klasa, igValue);
                        ig2.Add(kvp);
                        if (!ig.ContainsValue(igValue))
                        {
                            ig.Add(klasa, igValue);
                        }
                        return igValue;

                    }

                    else return 0.0;
                }
                return 0.0;
            }

            // entropia dla calego zestawu danych
            double maxIg = entropyForClass(rosliny, header);






            ///////////////////////////////////////////////////////////////
            // Entropia i Ig dla atrybutow //
            ///////////////////////////////////////////////////////////////
            double entropyForAttributes(List<Plant> list, string[] atrybuty)
            {
                double entropy = 0.0;
                double igValue = 0.0;
                double fract = 0.0;
                double numberPositive = 0.0;
                double numberNegative = 0.0;
                double sumExamples = 0.0;
                string[] valString;

                foreach (string a in atrybuty)
                {
                    int counter = atrybuty.Count();

                    // dla kazdego parametru osobny if znowu?
                    valString = rosliny.Select(o => o.lacksWater).Distinct().ToArray();

                    foreach (var value in valString)
                    {
                        for (int i = 0; i < counter; i++)
                        {
                            if (list[i].lacksWater == value && rosliny[i].harvest == true)
                                ++numberPositive;
                            if (list[i].lacksWater == value && rosliny[i].harvest == false)
                                ++numberNegative;
                        }
                        sumExamples = numberPositive + numberNegative + eps;

                        //jezeli tylko true albo false to zwracam liścia - 0 - false, 1 - true
                        if (numberPositive == 0)
                        {
                            return 0.0;
                        }
                        else if (numberNegative == 0)
                        {
                            return 1.0;
                        }

                        double fract01 = numberPositive / sumExamples + eps; // pozytywne / wszystkie
                        double fract02 = numberNegative / sumExamples + eps; // negatywne / wszystkie
                        double entropyAttribute = -fract01 * Math.Log(fract01, 2.0) - fract02 * Math.Log(fract02, 2.0);

                        //Console.WriteLine("entropia atrybutu " + value + " " + entropyAttribute);
                        entropy += entropyAttribute * (sumExamples / counter);
                        //Console.WriteLine("Entropia klasy: " + h + " " + entropy);

                        numberPositive = 0.0;
                        numberNegative = 0.0;
                        entropyAttribute = 0.0;

                        return igValue;
                    }


                    return 0.0;
                }




                //////////////////////////////////////////////////////////
                // szukamy atrybutu o najwiekszym przyroscie informacji //
                //////////////////////////////////////////////////////////
                string findRootClass(Dictionary<string, double> igValues)
                {
                    double maxValue = igValues.Max(mv => mv.Value);
                    string keyMax = igValues.FirstOrDefault(x => x.Value == maxValue).Key;
                    ig.Clear();
                    return keyMax;
                }
                //findRootClass(ig);





                /////////////////////
                // budujemy drzewo //
                /////////////////////

                void id3tree(List<Plant> plants, Dictionary<string, double> igValues)
                {
                    string[] branches;

                    string root = findRootClass(ig);
                    Tree<string> tree = new Tree<string>(root);
                    ig.Clear();

                    // galezie - "corn", "name"; "colza", "name"; "50", "height"; "50" "No";
                    Dictionary<string, string> oneBranch = new Dictionary<string, string>();
                    // do drzewka
                    List<Dictionary<string, string>> allBranches = new List<Dictionary<string, string>>();

                    // dodaje atrybuty roota jako galezie
                    if (root == "name") branches = rosliny.Select(o => o.name).Distinct().ToArray(); // "corn", "colza"
                    else if (root == "height") branches = rosliny.Select(o => o.height).Distinct().ToArray(); // "60", 120"
                    else if (root == "color") branches = rosliny.Select(o => o.color).Distinct().ToArray(); // "yellow", "brown"
                    else if (root == "isSick") branches = rosliny.Select(o => o.isSick).Distinct().ToArray(); // "true", "false"
                    else branches = rosliny.Select(o => o.lacksWater).Distinct().ToArray(); // "true, "false"

                    // string attributeToRemove = 
                    // branches = branches.Where(val => val = attributeToRemove).ToArray();

                    // dodaje wszystkie atrybuty dla klasy
                    foreach (var b in branches)
                    {
                        oneBranch.Add(b, root);
                        maxIg = entropyForClass(rosliny, header);
                        Console.WriteLine(b, root);

                    }
                    branches = branches.Where(val => val == root).ToArray();

                    while (branches.Length > 0)
                    {
                        id3tree(plants, ig);
                    }

                    tree.TraverseDFS();

                }
                id3tree(rosliny, ig);











                //////////////////////////////////////////////////////
                void testDrzewka()
                {
                    Tree<string> drzewko1 = new Tree<string>
                                 ("colza", new Tree<string>("80",
                                    new Tree<string>("No")));
                    Tree<string> drzewko2 = new Tree<string>("corn",
                                    new Tree<string>("160",
                                      new Tree<string>("true",
                                         new Tree<string>("Yes"),
                                  new Tree<string>("tulip",
                                    new Tree<string>("90",
                                      new Tree<string>("true",
                                        new Tree<string>("false",
                                          new Tree<string>("Yes")))))))
                        );
                    drzewko1.TraverseDFS();
                    drzewko2.TraverseDFS();

                }
                //testDrzewka();












                // klasa i atrybut, np "name", "corn"
                Dictionary<string, string> nodes = new Dictionary<string, string>();
                // budujemy 'drzewo' i rozgalezienia - key = winner, value = atrybuty
                // szukamy kolejnego node'a - klasy o najwiekszym IG  dla kazdego atrybutu po obecnej klasie
                void findNextAttribute(List<Plant> lista) //, string klasaSzukaj, string[] klasaOpusc, string atrybut)
                {

                    double numberOne = 0.0;
                    double numberTwo = 0.0;

                    double igValue = 0.0;


                    // jeżeli wszystkie przyklady sa pozytywne albo negatywne to konczymy i dodajemy node'a "koniec"
                    if (lista.Select(o => o.harvest).Distinct().ToArray().Length < 2)
                    {
                        nodes.Add("end", lista.Select(o => o.harvest).Distinct().ToString());
                        return;
                    }


                }


                // Lista rosliny List<Plant>
                // string: klasa - ktorej atrybutow szukamy, np "name"
                // string: wartosc - szukamy wszystkich wartosci danej klasy, np "name" : colza, corn, wheat, tulip
                // liczymy przyrost informacji dla kazdej z pozostalych klas 











                // szukamy entropii dla poszczegolnych atrybutow dla klasy o najwiekszym IG:
                // foreach atrybut z klasy o najwyzszym IG:                 height: 50
                //      jeżeli atrybuty maja rozne wartosci T/F dla klasy 'harvest':           
                //          wydzielamy podtablice ze wszystkimi atrybutami pozostalych klas, gdzie height = 50, bez height
                //          liczymy entropie i IG dla kazdego atrybutu z podtablicy, poza height,       color ma najwiekszy IG, 
                //          atrybut o najwyższym IG staje sie kolejnym nodem, te klase usuwamy z listy
                //          jeżeli atrybuty maja rozne wartosci T/F dla klasy 'harvest':
                //          ....
                //      jeżeli nie, to zwracamy (dodajemy do node'a) jedyną wartość dla tego atrybutu - Trua albo False i konczymy z atrybutem
                void attributesEntropy(string atrybut)
                {


                }


                // szukam 'podtablicy' dla danego atrybutu
                void getSubTable(string attribute)
                {
                    Dictionary<string, double> atrybuty = new Dictionary<string, double>();

                }


                // przewidywanie, klasyfikuje jako true/false po przejsciu drzewa
                bool predictOutcome(Plant roslinaNaPlaszy)
                {
                    // dopasowuje kazda wartosc z tablicy przekazanej jako parametr z odpowiednimi wartosciami 
                    //  w utworzonym drzewie i ide drzewkiem do konca, az sie skoncza wartosci lub napotkam true albo false
                    // i zwracam bool true albo false

                    return true;
                }


                string node = "";
                Tree<string> drzewo(List<string> features, string[] vals)
                {

                    if (features.Count > 0)
                    {
                        for (int i = 0; i < features.Count; i++)
                        {

                            foreach (string v in vals)
                            {
                                node += v + "\n";
                            }
                            node += "\t";
                            header.Remove(header.First());
                            drzewo(features, allValues);
                        }
                    }
                    else
                    {
                        Console.WriteLine(node);
                    }

                    return new Tree<string>(node);
                }

                // drzewo(header, allValues);




                Console.WriteLine("finito.");
                Console.ReadLine();








            }
        }

    }

