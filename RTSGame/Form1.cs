using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Media;
    
namespace RTSGame
{
    public partial class GameScreen : Form
    {
        Random rng = new Random();
        double p2SpawnCountdown = 0, p2SpawnLimit;
        int p1L1ArrowCounter, p1L2ArrowCounter, p1L3ArrowCounter, p2L1ArrowCounter, p2L2ArrowCounter, p2L3ArrowCounter;
        int p1Health, p2Health;  // the health of each player, and the selected lane of each
        int p1SelectedLane = 2, p2SelectedLane = 2;
        long p1Resources, p2Resources;      // resources. p2Resources is unused if the below is true.
        Boolean theSecondPlayerIsARobot = false;      // if this is true, then Player 2 is a robot. Intialize AI.
        Boolean p1RangedL1 = false, p1RangedL2 = false, p1RangedL3 = false, p2RangedL1 = false, p2RangedL2 = false, p2RangedL3 = false; // these check if there's a ranged goon in each lane for each player
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, bDown, nDown, mDown, spaceDown; // P1 Key controls
        Boolean aDown, sDown, dDown, wDown, cDown, vDown, xDown, zDown; //P2 Key Controls (if P2 is a player)

        List<string> P1Goons = new List<string>(); // player 1's goons. Max of 10.
        List<int> P1GoonsX = new List<int>();
        List<int> P1GoonsY = new List<int>();
        List<int> P1GoonsHealth = new List<int>();
        List<string> P2Goons = new List<string>();     // Player 2's goons. Max of 10 also.
        List<int> P2GoonsX = new List<int>();
        List<int> P2GoonsY = new List<int>();
        List<int> P2GoonsHealth = new List<int>();
        List<string> P1Arrows = new List<string>();   // Arrows, I guess.
        List<int> P1ArrowsX = new List<int>();
        List<int> P1ArrowsY = new List<int>();
        List<string> P2Arrows = new List<string>();     // Also arrows, but for player 2.
        List<int> P2ArrowsX = new List<int>();
        List<int> P2ArrowsY = new List<int>();

        public GameScreen()
        {
            InitializeComponent();
            p2SpawnLimit = rng.Next(5, 11) * 50;
            p1Health = 100;
            p2Health = 100;
            p1L1ArrowCounter = 0;
            p1L2ArrowCounter = 0;
            p1L3ArrowCounter = 0;
        }
        /// <summary>
        /// This is the Game Engine, running at approx. 50fps. Everything that runs constantly goes here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if(theSecondPlayerIsARobot == true) {p1Resources++; p2Resources = 0; }
            else { p1Resources++; p2Resources++; }

            if(theSecondPlayerIsARobot == true) { CPU_AI(); }
            p1ResourceLabel.Text = "Resources: " + p1Resources;
            p2ResourceLabel.Text = "Resources: " + p2Resources;
            p1HealthLabel.Text = "Health: " + p1Health;
            p2HealthLabel.Text = "Health: " + p2Health;
            if (p1RangedL1 == true) { p1L1ArrowCounter++; }         //These are the mundane tasks, counting, mostly.
            if (p1RangedL2 == true) { p1L2ArrowCounter++; }
            if (p1RangedL3 == true) { p1L3ArrowCounter++; }
            if (p2RangedL1 == true) { p2L1ArrowCounter++; }
            if (p2RangedL2 == true) { p2L2ArrowCounter++; }
            if (p2RangedL3 == true) { p2L3ArrowCounter++; }

            GoonAI();
            P1GoonSpawning();
            LaneSelection();                                  //Calls up the various functions
            if (theSecondPlayerIsARobot == true) { CPU_AI(); }
            else { P2GoonSpawning(); }
            WinAndLose();
            Refresh();
        }

        public void WinAndLose()
        {
            if (p1Health <= 0)
            {
                GameTimer.Enabled = false;
                p2HealthLabel.Text = "Win!";            }
            else if (p2Health <= 0)
            {
                GameTimer.Enabled = false;
                p1HealthLabel.Text = "Win!";
            }
        }

        /// <summary>
        /// Semi-generic AI scripts for Goon units
        /// </summary>
        public void GoonAI( /* parametre 1, parametre 2 */)
        {
            int p1G = 0;
            int p2G = 0;
            int p1A = 0;        //initilizes the goon scripts.
            int p2A = 0;
            for (; p1G < P1Goons.Count(); p1G++)
            {
                /* Hey look: a Psuedosummary! What I want here is for each goon in the p1Goonlist that isn't 
                   ranged to move right. When they intersect with another (enemy) goon, stop and fight. Health is stored.
                   When/If they hit the other end of the screen, despawn and drop fifteen health from P2.*/
                if(P1Goons[p1G] == "M")
                {
                    P1GoonsX[p1G]++;        // moves the goon to the right
                  
                   if(P1GoonsX[p1G] > 500)
                   {
                        P1Goons.RemoveAt(p1G);
                        P1GoonsX.RemoveAt(p1G);         // once the goon gets to a certain point on the scree, wipe it and remove 15 health from P2.
                        P1GoonsY.RemoveAt(p1G);
                        P1GoonsHealth.RemoveAt(p1G);
                        p2Health = p2Health - 15;
                        break;
                   }
                  
                }
                for (; p1A < P1Arrows.Count(); p1A++)
                {
                    P1ArrowsX[p1A] = P1ArrowsX[p1A] + 3 + (1/2);
                    if(P1ArrowsX[p1A] > 450)
                    {
                        P1Arrows.RemoveAt(p1A);             // Same as above but for P1's arrows.
                        P1ArrowsX.RemoveAt(p1A);
                        P1ArrowsY.RemoveAt(p1A);

                    }
                }

            }
            for (; p2G < P2Goons.Count; p2G++)
            {
                if (P2Goons[p2G] == "M")
                {
                    P2GoonsX[p2G]--;

                    if (P2GoonsX[p2G] < 5)
                    {
                        P2Goons.RemoveAt(p2G);          // Moves player 2 melee goons to the other end of the screen. Wipes them when they get there, drop 15 health from P1.
                        P2GoonsX.RemoveAt(p2G);
                        P2GoonsY.RemoveAt(p2G);
                        P2GoonsHealth.RemoveAt(p2G);
                        p1Health = p1Health - 15;
                        break;
                    }
                }
            }
            for (; p2A < P2Arrows.Count(); p2A++)
            {
                P2ArrowsX[p2A] = P2ArrowsX[p2A] - 3 - (1 / 2);
                if (P2ArrowsX[p2A] < 50)
                {
                    P2Arrows.RemoveAt(p2A);                         // same as above but for arrows.
                    P2ArrowsX.RemoveAt(p2A);
                    P2ArrowsY.RemoveAt(p2A);

                }
            }

            for (int i = 0; i < P1Goons.Count; i++)
            {
                Rectangle P1Goon = new Rectangle(P1GoonsX[i], P1GoonsY[i], 20, 20);

                for (int j = 0; j < P2Goons.Count; j++)                         // Collision between P1 & P2 Goons. Doesn't work for some reason.
                {
                    Rectangle P2Goon = new Rectangle(P2GoonsX[j], P2GoonsY[j], 20, 20);

                    if (P1Goon.IntersectsWith(P2Goon))
                    {
                        P1GoonsHealth[i] = P1GoonsHealth[i] - 5;        // The goons will "fight" if they meet.
                        P2GoonsHealth[j] = P2GoonsHealth[j] - 5;

                        if (P1GoonsHealth[i] <= 0)
                        {
                            if (P1Goons[i] == "R")
                            {
                                if (P1GoonsY[i] == 30) { p1RangedL1 = false; }
                                else if (P1GoonsY[i] == 165) { p1RangedL2 = false; }    // Extra stuff for ranged goons.
                                else { p1RangedL3 = false; }
                            }
                            P1Goons.RemoveAt(i);
                            P1GoonsX.RemoveAt(i);               // Wipes the goon if it dies.
                            P1GoonsHealth.RemoveAt(i);
                            P1GoonsY.RemoveAt(i);
                        }
                        if (P2GoonsHealth[j] <= 0)
                        {
                            if (P2Goons[j] == "R")
                            {
                                if (P2GoonsY[j] == 30) { p2RangedL1 = false; }
                                else if (P2GoonsY[j] == 165) { p2RangedL2 = false; }
                                else { p2RangedL3 = false; }            // This is the death script for P2.
                            }
                            P2Goons.RemoveAt(j);
                            P2GoonsX.RemoveAt(j);
                            P2GoonsHealth.RemoveAt(j);
                            P2GoonsY.RemoveAt(j);
                        }
                    }
                }
            }
                for (int k = 0; k < P1Arrows.Count; k++)
                {
                    Rectangle P1Arrow = new Rectangle(P1ArrowsX[k], P1ArrowsY[k], 5, 30);

                    for (int j = 0; j < P2Goons.Count; j++)         //Collision between arrows and goons. Also doesn't work for some reason.
                    {
                        Rectangle P2Goon = new Rectangle(P2GoonsX[j], P2GoonsY[j], 20, 20);

                        if (P1Arrow.IntersectsWith(P2Goon))
                        {
                            P2GoonsHealth[j] = P2GoonsHealth[j] - 2;
                            P1Arrows.RemoveAt(k);       // arrows are wiped as soon as they hit something that isn't another arrow.
                            P1ArrowsX.RemoveAt(k);
                            P1ArrowsY.RemoveAt(k);
                            if (P2GoonsHealth[j] <= 0)
                            {
                                P2Goons.RemoveAt(j);
                                P2GoonsX.RemoveAt(j);
                                P2GoonsY.RemoveAt(j);       //P2 goon death script.
                                P2GoonsHealth.RemoveAt(j);
                            }
                            break;
                        }
                    }

                }
            for (int h = 0; h < P2Arrows.Count; h++)
            {
                Rectangle P2Arrow = new Rectangle(P2ArrowsX[h], P2ArrowsY[h], 30, 5);

                for (int i = 0; i < P1Goons.Count; i++)            //The Player 2 arrows script. Same as above, but directions are reversed.
                {
                    Rectangle P1Goon = new Rectangle(P1GoonsX[i], P1GoonsY[i], 20, 20);
                    if (P2Arrow.IntersectsWith(P1Goon))
                    {
                        P1GoonsHealth[i] = P1GoonsHealth[i] - 2;
                        P2Arrows.RemoveAt(h);
                        P2ArrowsX.RemoveAt(h);
                        P2ArrowsY.RemoveAt(h);

                        if (P1GoonsHealth[i] <= 0)
                        {
                            P1Goons.RemoveAt(i);
                            P1GoonsX.RemoveAt(i);
                            P1GoonsY.RemoveAt(i);
                            P1GoonsHealth.RemoveAt(i);
                        }
                        break;
                    }
                }
            }    
            
        }

        /// <summary>
        /// Goonspawning script for Player 1. Always active.
        /// </summary>
        public void P1GoonSpawning()
        {
            if (bDown == true)     //Check: right arrow spawns a melee Goon.
            {
                if (p1Resources >= 100)         //Check: Does P1 have suffient resources to spawn a goon?
                {
                    if (P1Goons.Count < 10)         //Check: Does P1 have 10 goons already? (starts at 0)
                    {
                        if (p1SelectedLane == 1)
                        {
                            p1Resources = p1Resources - 100;
                            P1Goons.Add("M");
                            P1GoonsHealth.Add(50);
                            P1GoonsX.Add(40);               // Spawns a Melee in Lane 1
                            P1GoonsY.Add(30);
                        }
                        else if (p1SelectedLane == 2)
                        {
                            p1Resources = p1Resources - 100;
                            P1Goons.Add("M");
                            P1GoonsHealth.Add(50);          //Spawns a Melee in Lane 2
                            P1GoonsX.Add(40);
                            P1GoonsY.Add(165);
                        }
                        else if (p1SelectedLane == 3)
                        {
                            p1Resources = p1Resources - 100;
                            P1Goons.Add("M");
                            P1GoonsHealth.Add(50);              // "" "" in lane 3
                            P1GoonsX.Add(40);
                            P1GoonsY.Add(295);
                        }
                    }
                }
                else { }
                return;
            }
            if (mDown == true)      //Ranged goons are spawned with the left arrow.
            {
                if (p1Resources >= 100)
                {
                    if (P1Goons.Count < 10)
                    {
                        if (p1SelectedLane == 1)
                        {
                            if (p1RangedL1 == false)        //Also checks if there is already a Ranged goon in that lane. 
                            {
                                p1Resources = p1Resources - 100;
                                P1Goons.Add("R");
                                P1GoonsHealth.Add(1);
                                P1GoonsX.Add(30);       // Spawns a Ranged in Lane 1
                                P1GoonsY.Add(30);
                                p1RangedL1 = true;
                            }
                        }
                        else if (p1SelectedLane == 2)
                        {
                            if (p1RangedL2 == false)
                            {
                                p1Resources = p1Resources - 100;
                                P1Goons.Add("R");
                                P1GoonsHealth.Add(1);       // Spawns a Ranged in Lane 2
                                P1GoonsX.Add(30);
                                P1GoonsY.Add(165);
                                p1RangedL2 = true;
                            }
                        }
                        else if (p1SelectedLane == 3)
                        {
                            if (p1RangedL3 == false)
                            {
                                p1Resources = p1Resources - 100;
                                P1Goons.Add("R");
                                P1GoonsHealth.Add(1);           /// """ """ "" Lane 3
                                P1GoonsX.Add(30);
                                P1GoonsY.Add(295);
                                p1RangedL3 = true;
                            }
                        }
                    }
                }
                else { }
            }
            if (p1RangedL1 == true && p1L1ArrowCounter >= 100)
            {
                p1L1ArrowCounter = 0;
                P1Arrows.Add("A");          //Lane 1 arrow spawning. Only works if a ranged goon is present.
                P1ArrowsX.Add(30);
                P1ArrowsY.Add(37);
            }
         else if (p1RangedL2 == true && p1L2ArrowCounter >= 100)
            {
                p1L2ArrowCounter = 0;
                P1Arrows.Add("A");          // Lane 2 """ " "" "" "" 
                P1ArrowsX.Add(30);
                P1ArrowsY.Add(172);
            }
            else if (p1RangedL3 == true && p1L3ArrowCounter >= 100)
            {
                p1L3ArrowCounter = 0;
                P1Arrows.Add("A");
                P1ArrowsX.Add(30);          // Lane 3 "" " "" " ""
                P1ArrowsY.Add(302);
            }
            return;
        }
        /// <summary>
        /// Player 2's Goon Spawning script
        /// </summary>
        public void P2GoonSpawning()
        {
            if (xDown == true)     //Check: right arrow spawns a melee Goon.
            {
                if (p2Resources >= 100)         //Check: Does P2 have suffient resources to spawn a goon?
                {
                    if (P2Goons.Count < 10)         //Check: Does P2 have 10 goons already? (starts at 0)
                    {
                        if (p2SelectedLane == 1)
                        {
                            p2Resources = p2Resources - 100;
                            P2Goons.Add("M");
                            P2GoonsHealth.Add(50);
                            P2GoonsX.Add(450);               // Spawns a Melee in Lane 1
                            P2GoonsY.Add(30);
                        }
                        else if (p2SelectedLane == 2)
                        {
                            p2Resources = p2Resources - 100;
                            P2Goons.Add("M");
                            P2GoonsHealth.Add(50);          //Spawns a Melee in Lane 2
                            P2GoonsX.Add(450);
                            P2GoonsY.Add(165);
                        }
                        else if (p2SelectedLane == 3)
                        {
                            p2Resources = p1Resources - 100;
                            P2Goons.Add("M");
                            P2GoonsHealth.Add(50);              // "" "" in lane 3
                            P2GoonsX.Add(450);
                            P2GoonsY.Add(295);
                        }
                    }
                }
                else { }
                return;
            }
            if (cDown == true)      //Ranged goons are spawned with the left arrow.
            {
                if (p2Resources >= 100)
                {
                    if (P2Goons.Count < 10)
                    {
                        if (p2SelectedLane == 1)
                        {
                            if (p2RangedL1 == false)        //Also checks if there is already a Ranged goon in that lane. 
                            {
                                p2Resources = p2Resources - 100;
                                P2Goons.Add("R");
                                P2GoonsHealth.Add(1);
                                P2GoonsX.Add(480);       // Spawns a Ranged in Lane 1
                                P2GoonsY.Add(30);
                                p2RangedL1 = true;
                            }
                        }
                        else if (p1SelectedLane == 2)
                        {
                            if (p2RangedL2 == false)
                            {
                                p2Resources = p1Resources - 100;
                                P2Goons.Add("R");
                                P2GoonsHealth.Add(1);       // Spawns a Ranged in Lane 2
                                P2GoonsX.Add(480);
                                P2GoonsY.Add(165);
                                p2RangedL2 = true;
                            }
                        }
                        else if (p2SelectedLane == 3)
                        {
                            if (p2RangedL3 == false)
                            {
                                p2Resources = p1Resources - 100;
                                P2Goons.Add("R");
                                P2GoonsHealth.Add(1);           /// """ """ "" Lane 3
                                P2GoonsX.Add(480);
                                P2GoonsY.Add(295);
                                p2RangedL3 = true;
                            }
                        }
                    }
                }
                else { }
            }
            if (p2RangedL1 == true && p2L1ArrowCounter >= 100)
            {
                p2L1ArrowCounter = 0;
                P2Arrows.Add("A");          //Lane 1 arrow spawning. Only works if a ranged goon is present.
                P2ArrowsX.Add(450);
                P2ArrowsY.Add(37);
            }
            else if (p2RangedL2 == true && p2L2ArrowCounter >= 100)
            {
                p2L2ArrowCounter = 0;
                P2Arrows.Add("A");          // Lane 2 """ " "" "" "" 
                P2ArrowsX.Add(450);
                P2ArrowsY.Add(172);
            }
            else if (p2RangedL3 == true && p2L3ArrowCounter >= 100)
            {
                p2L3ArrowCounter = 0;
                P2Arrows.Add("A");
                P2ArrowsX.Add(450);          // Lane 3 "" " "" " ""
                P2ArrowsY.Add(302);
            }
            return;
        }

        public void LaneSelection()
        {
            if (upArrowDown == true)
            {
                p1SelectedLane--;       // Goes down if downarrow is pressed.
            }
            else if (downArrowDown == true)
            {
                p1SelectedLane++;       //goes up if uparrow is pressed.
            }
            else if (leftArrowDown == true)
            {
                p1SelectedLane = 2;
            }
             if(p1SelectedLane > 3) { p1SelectedLane = 3; }     // these keep you from going past lanes 1 and 3.
             if(p1SelectedLane < 1) { p1SelectedLane = 1; }

            if (wDown == true)
            {
                p2SelectedLane--;       // Goes down if downarrow is pressed.
            }
            else if (sDown == true)
            {
                p2SelectedLane++;       //goes up if uparrow is pressed.
            }
            else if (dDown == true)
            {
                p2SelectedLane = 2;
            }
            if (p2SelectedLane > 3) { p2SelectedLane = 3; }     // these keep you from going past lanes 1 and 3.
            if (p2SelectedLane < 1) { p2SelectedLane = 1; }
        }



        /// <summary>
        /// The AI for Player 2 if Player 2 is CPU-controlled.
        /// </summary>
        public void CPU_AI()
        {
            int coinFlip;       // Coin flip: 1 or 2?
            p2SelectedLane = 0;     //defaults so P2 doesn't spawn 10 goons at once.
            p2SpawnCountdown = p2SpawnCountdown + 2;        //Counts up to a random number.
            if(p2SpawnCountdown >= p2SpawnLimit)        //CPU tries to spawn a goon after that number is hit.
            {
                p2SpawnCountdown = 0;
                    p2SpawnLimit = rng.Next(5, 11) * 50;       //Resets the counter and rngs a new end goal number.
                p2SelectedLane = rng.Next(1, 4);        //Randomly selects a lane.
            }
            if (p2SelectedLane == 1)        //Spawnscript for Lane 1
            {
                if (P2Goons.Count < 10)      //Can't spawn a goon if 10 are already present
                {
                    coinFlip = rng.Next(1, 3);
                    if (coinFlip == 1 && p2RangedL1 == false) // A flip of 1 spawns a ranged goon in Lane 1 
                    {
                        P2Goons.Add("R");
                        P2GoonsX.Add(480);
                        P2GoonsY.Add(30);
                        P2GoonsHealth.Add(1);
                        p2RangedL1 = true;
                        p2SpawnLimit = rng.Next(5, 11) * 100;
                    }
                    else
                    {
                        P2Goons.Add("M");
                        P2GoonsX.Add(450);          // Spawns a melee if it can't spawn a ranged, or flips a 2.
                        P2GoonsY.Add(30);
                        P2GoonsHealth.Add(50);
                        p2SpawnLimit = rng.Next(5, 11) * 100;
                    }
                }
            }
            else if (p2SelectedLane == 2)
            {
                if (P2Goons.Count < 10)
                {
                    coinFlip = rng.Next(1, 3);
                    if (coinFlip == 1 && p2RangedL2 == false)       //Same as above, but for lane 2
                    {
                        P2Goons.Add("R");
                        P2GoonsX.Add(480);
                        P2GoonsY.Add(165);
                        P2GoonsHealth.Add(1);
                        p2RangedL2 = true;
                        p2SpawnLimit = rng.Next(5, 11) * 100;
                    }
                    else
                    {
                        P2Goons.Add("M");
                        P2GoonsX.Add(450);
                        P2GoonsY.Add(165);
                        P2GoonsHealth.Add(50);
                        p2SpawnLimit = rng.Next(5, 11) * 100;
                    }
                }
            }
            else if (p2SelectedLane == 3)           //Same as above, but for lane 3.
            {
                if (P2Goons.Count < 10)
                {
                    coinFlip = rng.Next(1, 3);
                    if (coinFlip == 1 && p2RangedL3 == false)
                    {
                        P2Goons.Add("R");
                        P2GoonsX.Add(480);
                        P2GoonsY.Add(295);
                        P2GoonsHealth.Add(1);
                        p2RangedL3 = true;
                        p2SpawnLimit = rng.Next(5, 11) * 10;
                    }
                    else
                    {
                        P2Goons.Add("M");
                        P2GoonsX.Add(450);
                        P2GoonsY.Add(295);
                        P2GoonsHealth.Add(50);
                        p2SpawnLimit = rng.Next(5, 11) * 10;
                    }
                }
            }
            if (p2RangedL1 == true && p2L1ArrowCounter >= 100)      //P2 arrow-spawning for all lanes.
            {
                p2L1ArrowCounter = 0;
                P2Arrows.Add("A");
                P2ArrowsX.Add(445);
                P2ArrowsY.Add(37);
            }
            else if (p2RangedL2 == true && p2L2ArrowCounter >= 100)
            {
                p2L2ArrowCounter = 0;
                P2Arrows.Add("A");
                P2ArrowsX.Add(445);
                P2ArrowsY.Add(172);
            }
            else if (p2RangedL3 == true && p2L3ArrowCounter >= 100)
            {
                p2L3ArrowCounter = 0;
                P2Arrows.Add("A");
                P2ArrowsX.Add(445);
                P2ArrowsY.Add(302);
            }

            //end of AI.
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {                           //Keyboard controls.
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.M:
                    mDown = true;
                    break;
                case Keys.B:
                    bDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.C:
                    cDown = true;
                    break;
                case Keys.X:
                    xDown = true;
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {                               //Keyboard controls part 2
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.M:
                    mDown = false;
                    break;
                case Keys.B:
                    bDown = false;
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.C:
                    cDown = false;
                    break;
                case Keys.X:
                    xDown = false;
                    break;
            }
        }

        /// <summary>
        /// Graphics.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameField_Paint(object sender, PaintEventArgs e)       // The graphics engine, paints everything on the field.
        {
            Brush P1Brush = new SolidBrush(Color.Maroon);
            Brush P2Brush = new SolidBrush(Color.Navy);
            Brush ArrowBrush = new SolidBrush(Color.Black);

            for (int p1G = 0; p1G < P1Goons.Count(); p1G++)
            {
                if (P1Goons[p1G] == "M") { e.Graphics.FillRectangle(P1Brush, P1GoonsX[p1G], P1GoonsY[p1G], 20, 20); }
                else if (P1Goons[p1G] == "R") { e.Graphics.FillEllipse(P1Brush, P1GoonsX[p1G], P1GoonsY[p1G], 20, 20); }                
            }
            for (int p1A = 0; p1A < P1Arrows.Count(); p1A++)
            {
                 e.Graphics.FillRectangle(ArrowBrush, P1ArrowsX[p1A], P1ArrowsY[p1A], 30, 5); 
            }
            for (int p2A = 0; p2A < P2Arrows.Count(); p2A++)
            {
                 e.Graphics.FillRectangle(ArrowBrush, P2ArrowsX[p2A], P2ArrowsY[p2A], 30, 5); 
            }
            for (int p2G = 0; p2G < P2Goons.Count(); p2G++)
                {       
              if(P2Goons[p2G] == "M") { e.Graphics.FillRectangle(P2Brush, P2GoonsX[p2G], P2GoonsY[p2G], 20, 20); }
              else if (P2Goons[p2G] == "R"){ e.Graphics.FillEllipse(P2Brush, P2GoonsX[p2G], P2GoonsY[p2G], 20, 20); }

            }
            if (p1SelectedLane == 1){ e.Graphics.FillRectangle(P1Brush, 0, 10, 10, 50); }
            else if (p1SelectedLane == 2) { e.Graphics.FillRectangle(P1Brush, 0, 150, 10, 50); }
            else { e.Graphics.FillRectangle(P1Brush, 0, 280, 10, 50); }
            if (p2SelectedLane == 1) { e.Graphics.FillRectangle(P2Brush, 530, 10, 10, 50); }
            else if (p2SelectedLane == 2) { e.Graphics.FillRectangle(P2Brush, 530, 150, 10, 50); }
            else { e.Graphics.FillRectangle(P2Brush, 530, 280, 10, 50); }
        }
    }
}