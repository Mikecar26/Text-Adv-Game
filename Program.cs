
/*
Moveset:

attack Bandit
attack Dog_1
attack Dog_2
attack Dog_3
potion
skip

 */







class Program
{
    static void Main()
    {

        List<Fighter> turnOrder = new List<Fighter>();
        Player player = new Player(100f, 3, turnOrder);

        Enemy enemy1 = new Enemy(25, player, "Bandit");
        Enemy enemy2 = new Enemy(15, player, "Dog_1");
        Enemy enemy3 = new Enemy(15, player, "Dog_2");
        Enemy enemy4 = new Enemy(15, player, "Dog_3");

        turnOrder.Add(player);
        turnOrder.Add(enemy1);
        turnOrder.Add(enemy2);
        turnOrder.Add(enemy3);
        turnOrder.Add(enemy4);

        Console.WriteLine("A Bandit and his Three Dogs challenge you");
        Console.WriteLine("Tip: You can either type \"attack\" and then the \"enemy name\" \nYou can type \"potion\" to heal\nOr type\"skip\" to skip your turn");

        Console.Write("Turn Order: \n");
        foreach (Fighter fighter in turnOrder)
        {
            Console.Write(fighter.name + " ");
            Console.WriteLine();
        }


        int enemyKilled;

        do
        {
            enemyKilled = 0;

            foreach (Fighter fighter in turnOrder)
            {

                if (fighter.GetType() == typeof(Player))
                {
                    int option;

                    Console.WriteLine("-------Your Turn-------");
                    Console.WriteLine("Players Health: " + fighter.currentHealth + "\nAmount of Potions: " + player.potions);

                    while (true)
                    {
                        string choice = Console.ReadLine();
                        option = fighter.TakeAction(choice);
                        if (option == 1)
                            break;
                        if (option == 0)
                            Console.WriteLine("Sorry you can't do that action! Try again");
                    }

                }
                else
                {
                    if (fighter.isDead)
                    {
                        enemyKilled++;
                        continue;
                    }
                    Console.WriteLine("-------" + fighter.name + "'s Turn-------");
                    fighter.TakeAction("attack player");
                }
            }

        }
        
while (!(((turnOrder.Count() - enemyKilled) == 1) || player.isDead));
        if (player.isDead)
            Console.WriteLine("You've Died!");
        else
            Console.WriteLine("You've Won!");
    }
    abstract class Fighter
    {
        
        // state
        public bool isDead;
        // stats
        public string name;
        public float maxHealth;
        public float currentHealth;
        // opponent
        public Fighter opponent;

        // methods
        public void TakeDamage(float dmg)
        {
            if (!isDead) {currentHealth -= dmg;}
            Console.WriteLine(this.name + " has taken " + dmg + " points of damage!");
            if (currentHealth <= 0) 
            {
                SetDead();
                Console.WriteLine("----------------" + this.name.ToUpper() + " HAS DIED!----------------");
            }
        }
        void SetDead() => isDead = true;

        public abstract int TakeAction(string choice);

        public Fighter() => currentHealth = maxHealth;

    }
    class Enemy : Fighter
    {
        public Enemy(float health, Player player, string name)
        {
            maxHealth = health;
            currentHealth = maxHealth;
            opponent = player;
            this.name = name;
        }
        public override int TakeAction(string choice)
        {
            opponent.TakeDamage(5f);
            Console.WriteLine(this.name + " has attacked at the player!");
            return 1;
        }
    }
    class Player : Fighter
    {
        public List<Fighter> allFighters;
        public int potions;
        public Player(float health, int potions , List<Fighter> fighters)
        {
            maxHealth = health;
            currentHealth = maxHealth;
            allFighters = fighters;
            this.potions = potions;
            this.name = "Player";
        }
        public override int TakeAction(string choices)
        {
            string [] vs = choices.Split(' ');
            string action = vs[0];
            string enemy = "";
            action.ToLower();
            try
            {
                enemy = vs[1];

            } catch(Exception)
            { };

            if (action.Contains("attack"))
            {
                foreach (Fighter fighter in allFighters)
                {
                    //if (fighter.name == null) { return 0; }

                    if (fighter.name == enemy)
                    {
                        if (fighter.isDead)
                            return 0;
                        opponent = fighter;
                    }
                }
                if (opponent == null) { return 0; }
                opponent.TakeDamage(5f);
                return 1;
            }
            else if (choices.Contains("potion"))
            {
                if (potions > 0)
                {
                    currentHealth += 100f;
                    potions--;
                    return 1;
                }

            }
            else if (choices.Contains("skip"))
            {
                return 1;
            }
            return 0;
        }
    }
}

