using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Cooking1
{
    /// <summary>
    /// classe Commande Fournisseur qui va ensuite être sérialisée pour créer un fichier xml
    /// fichier contenant les informations de la liste Commande Fournisseur (fournisseur, produit, quantité à commander)
    /// </summary>
    public class CommandeFournisseur
    {
        public List<string> listeCommandeFournisseur { get; set; }
    }

    class Program
    {
        static void Connexion_BDD1(string commande)
        {
            // Bien vérifier, via Workbench par exemple, que ces paramètres de connexion sont valides !!!
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Basket1204;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande; // exemple de requete bien-sur !

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            /* exemple de manipulation du resultat */
            string reponse_requete = "";
            while (reader.Read())                           // parcours ligne par ligne
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)    // parcours cellule par cellule
                {
                    string valueAsString = reader.GetValue(i).ToString();  // recuperation de la valeur de chaque cellule sous forme d'une string (voir cependant les differentes methodes disponibles !!)
                    currentRowAsString += valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);    // affichage de la ligne (sous forme d'une "grosse" string) sur la sortie standard
                reponse_requete += currentRowAsString;
            }
            connection.Close();
        }

        static void Connexion_BDD2(string commande)
        {
            // Bien vérifier, via Workbench par exemple, que ces paramètres de connexion sont valides !!!
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Basket1204;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande; // exemple de requete bien-sur !

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            connection.Close();
        }

        static void Fonctiontest()
        {
            //Test1();
            //Test2();
            //Test3();
            //Test4();
            //Test5();

        }

        static void Test1()
        {
            Connexion_BDD1(" select * from client;");
            Console.WriteLine("");
            Connexion_BDD1(" select * from utilise;");
            Console.WriteLine("");
            Connexion_BDD1(" select * from fournisseur;");
            Console.WriteLine("");
            Connexion_BDD1(" select * from produit;");
            Console.WriteLine("");
            Connexion_BDD1(" select * from recette;");
            Console.WriteLine("");
            Connexion_BDD1(" select * from client;");
            Console.WriteLine("");

            Connexion_BDD2("INSERT INTO `cooking`.`client` (`pseudo`,`motDePasse`,`prenomClient`,`nomClient`,`numeroTelClient`,`creditCook`,`CdR`) VALUES ('CP', 'cp3', 'Chris', 'Paul', '0655678899', 10, FALSE);");
            Connexion_BDD1(" select * from client;");
            Console.WriteLine("");

            ResetBase();
            Connexion_BDD1(" select * from client;");
        }

        static void Test2()
        {
            /*bool resulat = IdentifiantExiste("Drake");
             Console.WriteLine(resulat);*/

            /*List<string> mdp = ConnexionBDDDeuxColonne("select pseudo, motDePasse from client;"); 
            for (int i = 0; i < mdp.Count; i += 2)
            {
                Console.WriteLine(mdp[i] + mdp[i + 1]);
            }*/

            /*bool resultat2 = MdpExiste("rw", "RW");
            Console.WriteLine(resultat2);*/
        }

        static void Test3()
        {
            /*List<string> nbClient = ConnexionBDDUneColonne("select count(*) from client;");
            Console.WriteLine(nbClient[0]);

            List<string> nbCdR = ConnexionBDDUneColonne("select count(*) from client where CdR = true;");
            Console.WriteLine("Le nombre de client est" + nbCdR[0]);*/

            /*List<string> nomCdrNbRecette = ConnexionBDDDeuxColonne("select C.pseudo, sum(R.compteur) from client C, recette R where R.Pseudo = C.Pseudo group by C.pseudo;");
            for (int i = 0; i < nomCdrNbRecette.Count; i += 2)
            {
                Console.WriteLine("Le cuisinier : " + nomCdrNbRecette[i] + "a un nombre totale de recette commandé égale a :" + nomCdrNbRecette[i + 1]);
            }*/

            /*Console.WriteLine("\n Vous allez maintenant voir les produits dont le stock actuel est inférieur ou égale au stock minimal");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            List<string> listeProduit = ConnexionBDDUneColonne("select nomProduit from produit where stockActuel <= stockMinimal;");
            for (int i = 0; i < listeProduit.Count; i++)
            {
                Console.WriteLine("Le produit : " + listeProduit[i] + " a un stock actuel qui est inférieur ou égale a son stock minimal");
            }*/

            /*//Liste des produits ayant une quantité de stock <= leur quantité minimale
            Console.WriteLine("\n Vous allez maintenant voir les produits dont le stock actuel est inférieur ou égale au stock minimal");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            List<string> listeProduit = ConnexionBDDUneColonne("select nomProduit from produit where stockActuel <= stockMinimal;");
            for (int i = 0; i < listeProduit.Count; i++)
            {
                Console.WriteLine("Le produit : " + listeProduit[i] + " a un stock actuel qui est inférieur ou égale a son stock minimal");
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            // Saisie au clavier (par l'evaluateur) d'un des produits de la liste précédente puis affichage de la liste de leur recette (leur nom)
            Console.WriteLine("\n Vous avez maintenant la possibilité de saisir l'un des produit au dessus, afin d'en afficher la liste des recettes où il est présent");
            Console.WriteLine("Saisir le produit >");
            string produit = Console.ReadLine();
            bool produitCorrect = ProduitDansListe(produit, listeProduit);
            while (produitCorrect == false)
            {
                Console.WriteLine("\nRéponse incorrect");
                Console.WriteLine("Saisir le produit >");
                produit = Console.ReadLine();
                produitCorrect = ProduitDansListe(produit, listeProduit);
            }
            Console.WriteLine("Voici la liste des recettes que le produit : " + produit + " compose :");
            string commande = "select R.nomRecette from utilise U, recette R where U.nomProduit = " + '\u0022' + produit + '\u0022' + " and U.numeroRecette = R.numeroRecette;";
            List<string> listeRecette = ConnexionBDDUneColonne(commande);
            for (int i = 0; i < listeRecette.Count; i++)
            {
                Console.WriteLine((i + 1) + "." + listeRecette[i]);
                Console.WriteLine("i vaut : " + i);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();*/
        }

        static void Test4()
        {
            /*string identifiant = "RW";
            bool reponse = false;
            List<string> pseudo = ConnexionBDDUneColonne("select CdR from client where pseudo = " + '\u0022' + identifiant + '\u0022' + " ;");
            if (pseudo[0] == "True")
            {
                reponse = true;
            }
            Console.WriteLine(reponse);*/

            /*string identifiant = "RW";
            List<string> solde = ConnexionBDDUneColonne("select creditCook from client where pseudo = " + '\u0022' + identifiant + '\u0022' + ";");
            Console.WriteLine("Votre solde est de " + solde[0] + " cook");*/

            /*string identifiant = "RW";
            Console.WriteLine("\nVous allez maintenant voir toutes vos recettes ainsi que leur nombre de commande");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            Console.WriteLine("");
            List<string> nomRecetteNbRecette = ConnexionBDDDeuxColonne("select R.nomRecette, R.compteur from recette R where R.Pseudo = " + '\u0022' + identifiant + '\u0022' + ";");
            for (int i = 0; i < nomRecetteNbRecette.Count; i += 2)
            {
                Console.WriteLine("La recette : " + nomRecetteNbRecette[i] + " a un nombre de commande de : " + nomRecetteNbRecette[i + 1]);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();*/
        }

        static void Test5()
        {
            DateTime dateToday = DateTime.Now;
            Console.WriteLine(dateToday);

            DateTime thisDate = DateTime.Now;
            CultureInfo culture = new CultureInfo("ja-JP");
            Console.WriteLine(culture);
            Console.WriteLine(thisDate.ToString("d", culture));
            string date = thisDate.ToString("d", culture);
            Console.WriteLine(date);
            string dateFinale = "";
            for (int i = 0; i < date.Length; i++)
            {
                if (i == 4 || i == 7)
                {
                    dateFinale += "-";
                }
                else
                {
                    dateFinale += date[i];
                }
            }
            Console.WriteLine(date);
            Console.WriteLine(".." + dateFinale + "..");
        }

        /// <summary>
        /// Permet de prendre une commande et de retourner une liste
        /// A utiliser quand la commande n'a qu'une seule colonne de resultat
        /// </summary>
        /// <param name="commande">commande a executer</param>
        /// <returns>liste composée des résultats de la commande</returns>
        static List<string> ConnexionBDDUneColonne(string commande)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Basket1204;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            List<string> resultat = new List<string>();
            string pseudo;
            while (reader.Read())// parcours ligne par ligne
            {
                pseudo = reader.GetString(0);  // récupération de la 1ère colonne
                resultat.Add(pseudo);
            }
            connection.Close();
            return resultat;
        }

        /// <summary>
        /// Permet de prendre une commande et de retourner une liste
        /// A utiliser quand la commande retourne seulement 2 colonnes
        /// Placera les elements de la ligne une cote a cote, puis la ligne 2 cote a cote, etc
        /// </summary>
        /// <param name="commande">commande a executer</param>
        /// <returns>liste composé des resultat de la commande</returns>
        static List<string> ConnexionBDDDeuxColonne(string commande)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Basket1204;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            List<string> resultat = new List<string>();
            string pseudo;
            string mdp;
            while (reader.Read())// parcours ligne par ligne
            {
                pseudo = reader.GetString(0); // récupération de la 1ère colonne 
                mdp = reader.GetString(1);   // récupération de la 2ème colonne 
                resultat.Add(pseudo);
                resultat.Add(mdp);
            }
            connection.Close();
            return resultat;
        }

        /// <summary>
        /// Permet de prendre une commande et de retourner une liste
        /// A utiliser quand la commande retourne seulement 3 colonnes
        /// Placera les elements de la ligne une cote a cote, puis la ligne 2 cote a cote, etc
        /// </summary>
        /// <param name="commande">commande a executer</param>
        /// <returns>liste composé des resultat de la commande</returns>
        static List<string> ConnexionBDDTroisColonne(string commande)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Basket1204;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            List<string> resultat = new List<string>();
            string pseudo;
            string pseudo1;
            string mdp;
            while (reader.Read())// parcours ligne par ligne
            {
                pseudo = reader.GetString(0); // récupération de la 1ère colonne 
                mdp = reader.GetString(1);
                pseudo1 = reader.GetString(2); // récupération de la 2ème colonne 
                resultat.Add(pseudo);
                resultat.Add(mdp);
                resultat.Add(pseudo1);
            }
            connection.Close();
            return resultat;
        }

        /// <summary>
        /// Permet de prendre une commande et de retourner une liste
        /// A utiliser quand la commande retourne 7 colonnes
        /// Placera les elements de la ligne une cote a cote, puis la ligne 2 cote a cote, etc
        /// </summary>
        /// <param name="commande">commande a executer</param>
        /// <returns>liste composé des resultat de la commande</returns>
        static List<string> ConnexionBDDSeptColonne(string commande)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Basket1204;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            List<string> resultat = new List<string>();
            string pseudo;
            string pseudo1;
            string mdp;
            string pseudo2;
            string pseudo3;
            string mdp1;
            string pseudo4;
            while (reader.Read())// parcours ligne par ligne
            {
                pseudo = reader.GetString(0);
                mdp = reader.GetString(1);
                pseudo1 = reader.GetString(2);
                pseudo2 = reader.GetString(3);
                mdp1 = reader.GetString(4);
                pseudo3 = reader.GetString(5);
                pseudo4 = reader.GetString(6);
                resultat.Add(pseudo);
                resultat.Add(mdp);
                resultat.Add(pseudo1);
                resultat.Add(pseudo2);
                resultat.Add(mdp1);
                resultat.Add(pseudo3);
                resultat.Add(pseudo4);
            }
            connection.Close();
            return resultat;
        }

        //---------------------- Permet de créer et vérifier un compte-------------------------------------

        //------------------------Permet de vérifier un compte---------------------------
        /// <summary>
        /// Permet de vérifier si l'identifiant est dans la BBD
        /// </summary>
        /// <param name="identifiant">identifiant</param>
        /// <returns>Vraie si existe, faux sinon</returns>
        static bool IdentifiantExiste(string identifiant)
        {
            bool reponse = false;
            List<string> pseudo = ConnexionBDDUneColonne("select pseudo from client;");
            foreach (string value in pseudo)
            {
                if (identifiant == value)
                {
                    reponse = true;
                }
            }
            return reponse;
        }

        /// <summary>
        /// Permet de vérfier si le mdp est le bon
        /// </summary>
        /// <param name="mdp">mot de passe</param>
        /// <param name="identifiant">identifiant</param>
        /// <returns>vraie si le mdp est bon, faux sinon</returns>
        static bool MdpExiste(string mdp, string identifiant)
        {
            bool reponse = false;
            List<string> listePseudoMdp = ConnexionBDDDeuxColonne("select pseudo, motDePasse from client;");
            for (int i = 0; i < listePseudoMdp.Count; i += 2)
            {
                if (identifiant == listePseudoMdp[i] && mdp == listePseudoMdp[i + 1])
                {
                    reponse = true;
                }
            }
            return reponse;
        }

        /// <summary>
        /// Fonction qui vérifie le compte lors de l'identification
        /// Utilisé qunad l'utilisateur vaut s'identifier
        /// </summary>
        /// <returns>l'identifiant du client</returns>
        static string VerifierCompte()
        {
            Console.Write("Veuillez rentrer votre identifiant > ");
            string identifiant = Console.ReadLine();
            bool identifiantCorrect = IdentifiantExiste(identifiant);
            //Si l'identifiant n'est pas correct on lui redemande
            while (identifiantCorrect == false)
            {
                Console.WriteLine("La réponse est incorrect");
                Console.Write("Veuillez rentrer votre identifiant > ");
                identifiant = Console.ReadLine();
                identifiantCorrect = IdentifiantExiste(identifiant);
            }
            Console.Write("Veuillez rentrer votre mot de passe > ");
            string mdp = Console.ReadLine();
            bool mdpCorrect = MdpExiste(mdp, identifiant);
            // Si le mot de passe est incorrect on lui redemande
            while (mdpCorrect == false)
            {
                Console.WriteLine("La réponse est incorrect");
                Console.Write("Veuillez rentrer votre mdp > ");
                mdp = Console.ReadLine();
                mdpCorrect = MdpExiste(mdp, identifiant);
            }
            return identifiant;
        }

        //--------------------------------Permet de créer un compte-------------------------------- 

        /// <summary>
        /// Permet de vérifier si l'id passé en paramètre est valide
        /// càd s'il n'existe pas dans la BDD et qu'il répond à tous les critères (pas d'apostrophe, pas d'espace...)
        /// </summary>
        /// <param name="newidentifiant">identifiant à tester</param>
        /// <returns>True si l'id est valide, false sinon</returns>
        static bool NewIdValide(string newidentifiant)
        {
            bool reponse = false;
            bool reponseContraintes = false;
            int compteur = 0;
            List<string> pseudo = ConnexionBDDUneColonne("select pseudo from client;");
            if ((newidentifiant.Length < 20) && (newidentifiant.Contains("'") == false) && (newidentifiant.Contains("`") == false) && (newidentifiant.Contains(" ") == false))
            {
                reponseContraintes = true;
            }
            foreach (string value in pseudo)
            {
                if (value == newidentifiant)
                {
                    compteur++;
                }
            }
            if (compteur != 1 && reponseContraintes == true)
            {
                reponse = true;
            }

            return reponse;
        }

        /// <summary>
        /// permet de vérifier si une string contient des chiffres ou carcatères indésirables à la création d'un nom ou prénom d'un nouvel indésirable
        /// </summary>
        /// <param name="chaine">string</param>
        /// <returns>booléen qui ditsi il contient oui ou non un de ces caractères</returns>
        static bool VerificationCaractères(string chaine)
        {
            bool reponse = true;
            int compteur = 0;
            for (int i = 0; i < chaine.Length; i++)
            {
                if (chaine[i] == 48 || chaine[i] == 49 || chaine[i] == 50 || chaine[i] == 51 || chaine[i] == 52 || chaine[i] == 53 || chaine[i] == 54 ||
                    chaine[i] == 55 || chaine[i] == 56 || chaine[i] == 57 || chaine[i] == ' ' || chaine[i] == 39 || chaine[i] == 96)
                {
                    compteur++;
                }
            }
            if (compteur != 0)
            {
                reponse = false;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de vérifier si le prénom passé en paramètre est valide
        /// càd qu'il répond à tous les critères (bon nombre de caractères, pas d'apostrophe, pas d'espace...)
        /// </summary>
        /// <param name="newprenom">prénom ou nom</param>
        /// <returns>True si le prénom ou le nom est valide, false sinon</returns>
        static bool NewPrenomNomValide(string newprenom)
        {
            bool reponse = false;
            if (newprenom.Length < 20 && VerificationCaractères(newprenom))
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de vérifier si le numéro passé en paramètre est valide
        /// càd qu'il est bien composé de 10 chiffres et pas d'autres caractères indésirables
        /// </summary>
        /// <param name="newnum">numéro</param>
        /// <returns>True si le numéro est valide, false sinon</returns>
        static bool NewNumValide(string newnum)
        {
            bool reponse = false;
            int compteur = 0;
            if (newnum.Length == 10)
            {
                if (newnum[0] == '0' && (newnum[1] == '6' || newnum[1] == '7'))
                {
                    for (int i = 2; i < newnum.Length; i++)
                    {
                        if (newnum[i] >= 48 && newnum[i] <= 57)
                        {
                            compteur++;
                        }

                    }
                }
            }
            if (compteur == 8)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de vérifier si le mot de passe passé en paramètre est valide
        /// càd qu'il répond à tous les critères (bon nombre de caractères, pas d'apostrophe, pas d'espace...)
        /// </summary>
        /// <param name="newMdp">mot de passe</param>
        /// <returns>True si le mot de passe est valide, false sinon</returns>
        static bool NewMdpValide(string newmdp)
        {
            bool reponse = false;
            if (newmdp.Length < 20 && newmdp.Contains(" ") == false && newmdp.Contains("'") == false && newmdp.Contains("`") == false)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet d'insérer un nouveau client dans la base de donnée avec les données rentrées en paramètre
        /// et retourne un booléen confirmant que le nouvel utilisateur a été ajouté à la base de données
        /// </summary>
        /// <param name="pseudo">pseudo</param>
        /// <param name="motDePasse">mot de passe</param>
        /// <param name="prenomClient">prénom</param>
        /// <param name="nomClient">nom</param>
        /// <param name="numeroTelClient">numéro de téléphone</param>
        /// <param name="creditCook">crédit cook</param>
        /// <param name="CdR">si c'est un cdr ou pas</param>
        /// <returns>True si le client a été créé, false sinon</returns>
        static bool InsererClient(string pseudo, string motDePasse, string prenomClient, string nomClient, string numeroTelClient, int creditCook, bool CdR)
        {
            bool clientcree = false;
            if (NewIdValide(pseudo) && NewMdpValide(motDePasse) && NewPrenomNomValide(prenomClient) && NewPrenomNomValide(nomClient) && NewNumValide(numeroTelClient))
            {
                Connexion_BDD2("INSERT INTO cooking.`client` (pseudo,`motDePasse`,`prenomClient`,`nomClient`,`numeroTelClient`,`creditCook`,`CdR`) VALUES ('" + pseudo + "', '" + motDePasse + "', '" + prenomClient + "', '" + nomClient + "', '" + numeroTelClient + "', '" + creditCook + "', " + CdR + "); ");
                clientcree = true;
            }
            return clientcree;
        }

        /// <summary>
        /// Fonction qui permet de créer un compte
        /// Utilisé quand l'utilisateur veut s'identifier et donc créer un compte
        /// </summary>
        /// <returns>l'identifiant du client créer</returns>
        static string CreerCompte()
        {
            // On demande de créer un nouvel identifiant
            Console.Write("\nVeuillez rentrer un identifiant de moins de 20 caractères > ");
            string newidentifiant = Console.ReadLine();
            bool newidentifiantCorrect = NewIdValide(newidentifiant);
            while (newidentifiantCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer un identifiant de moins de 20 caractères > ");
                newidentifiant = Console.ReadLine();
                newidentifiantCorrect = NewIdValide(newidentifiant);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            // On possède un bon identifiant
            // On demande maintenant de créer un mdp
            Console.Write("\nVeuillez rentrer un mot de passe de moins de 20 caractères > ");
            string newmdp = Console.ReadLine();
            bool newmdpCorrect = NewMdpValide(newmdp);
            while (newmdpCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer un mot de passe de moins de 20 caractères > ");
                newmdp = Console.ReadLine();
                newmdpCorrect = NewMdpValide(newmdp);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            // On possède maintenant un bon mdp
            // On veut maintenant créer le prénom
            Console.Write("\nVeuillez rentrer votre prénom > ");
            string newprenom = Console.ReadLine();
            bool newprenomCorrect = NewPrenomNomValide(newprenom);
            while (newprenomCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer votre prénom > ");
                newprenom = Console.ReadLine();
                newprenomCorrect = NewPrenomNomValide(newprenom);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            // On veut maintenant créer le nom
            Console.Write("\nVeuillez rentrer votre nom > ");
            string newnom = Console.ReadLine();
            bool newnomCorrect = NewPrenomNomValide(newnom);
            while (newprenomCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer votre nom > ");
                newnom = Console.ReadLine();
                newnomCorrect = NewPrenomNomValide(newnom);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            // On veut maintenant créer le numéro de telephone
            Console.Write("\nVeuillez rentrer votre numéro > ");
            string newnum = Console.ReadLine();
            bool newnumCorrect = NewNumValide(newnum);
            while (newnumCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer numéro > ");
                newnum = Console.ReadLine();
                newnumCorrect = NewNumValide(newnum);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            // On initialise ses crédits cook à zéro
            int newcredit = 0;

            // On demande au client s'il veut être Cdr
            // On initialise sa reponse à false
            bool CdR = false;
            Console.WriteLine("\nVoulez-vous être un CdR ?");
            Console.WriteLine("1. si oui");
            Console.WriteLine("2. si non");
            Console.Write("Tapez votre réponse > ");
            int reponse3 = Convert.ToInt32(Console.ReadLine());
            while (reponse3 > 2 || reponse3 < 1)
            {
                Console.WriteLine("\nVoulez-vous être un CdR ?");
                Console.WriteLine("1. si oui");
                Console.WriteLine("2. si non");
                Console.Write("Tapez votre réponse > ");
                reponse3 = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            // S'il veut être un Cdr, on change la reponse à true:
            if (reponse3 == 1)
            {
                CdR = true;
            }
            // On doit maintenant rentrer le client dans la base de données
            bool clientcree = InsererClient(newidentifiant, newmdp, newprenom, newnom, newnum, newcredit, CdR);
            if (clientcree == true)
            {
                Console.WriteLine("\nVotre compte a été créé avec succès !");
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();
            }
            return newidentifiant;
        }

        //-------------------------------------------------------------------------------------------------

        //-------------------Utilisé dans le menu cook une fois identifié--------------------
        /// <summary>
        /// Permet de savoir si le client passé en parametre est un Cdr ou non
        /// Utilisé quand l'utlisateur s'est idenetifié et il veut acceder au menu cooking
        /// </summary>
        /// <param name="identifiant">pseudo du client</param>
        /// <returns>Vraie si le client est un Cdr, faux sinon</returns>
        static bool ClientCdr(string identifiant)
        {
            bool reponse = false;
            List<string> pseudo = ConnexionBDDUneColonne("select CdR from client where pseudo = " + '\u0022' + identifiant + '\u0022' + " ;");
            if (pseudo[0] == "True")
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// permet de savoir si le nombre passé en parametre est bien compris entre 1 et 5 et si c est un int
        /// </summary>
        /// <param name="nbRecette">nombre de recette</param>
        /// <returns>vraie si correct, faux sinon</returns>
        static bool NbRecetteValide(int nbRecette)
        {
            bool reponse = false;
            if (nbRecette >= 1 && nbRecette <= 5 && (nbRecette is int))
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet au client rentré en parametre de passer une commande
        /// Utilisé quand un client s'est identifier comme Cdr et veut passer une commande
        /// Réalise toutes ces fonctionnalités :
        /// Choisir une/plusieurs recette
        /// Dire cb de fois il veut cette recette
        /// Regarde quelle quantité enlever à chaque produit à partir des noms des recettes
        /// Vérifier qu'il y a assez de produit une fois les recettes choisie
        /// Payer la commande -> vérifier assez de cook sur le compte, sinon incrémenter le compte
        /// Insérer une commande dans la base de donnée avec INSERT INTO
        /// Déduire le nb de cook sur le compte du client
        /// Rémunérer le/les Cdr concerné 
        /// Incrémenter les compteur des recettes
        /// Prix de vente de la recette augmente de 2 cook si le nombre de commande dépasse 10 (cette commande comprise)
        /// Prix de vente de la recette augmente de 5 cook si le nombre de commande dépasse 50 (cette commande comprise)
        /// Les stocks des produits sont décrémenter en fonction des recettes commandées
        /// </summary>
        /// <param name="identifiant">pseudo du client voulant passer une commande</param>
        static void PasserComande(string identifiant)
        {
            // --------------------------------------------------Choisir une/plusieurs recette-----------------------------------------------------------------
            //On initialise les valeurs de base
            // Coninuer permet de savoir si le client veut continuer de rajouter des recettes dans sa commande
            int continuer = 0;
            int reponsecontinuer = 0;
            // Stock le nom de toutes les recettes de la BDD
            List<string> recetteDisponible = ConnexionBDDUneColonne("select nomRecette from recette;");
            int compteur = 1;
            string nomRecette = "";
            int nbRecette = 0;
            bool nomRecetteValide = false;
            bool nbRecetteValide = false;
            // initialisation de la liste qui va stocker le nom des recettes commandé
            List<string> listeNomRecetteCommande = new List<string>();
            // initialisation de la liste qui va stocker la quantité de chaque recette commandée
            List<int> listeNbRecetteCommande = new List<int>();
            while (continuer == 0)
            {
                // Parcourir la liste des recettes et les proposer au client
                Console.WriteLine("Voici la liste des recettes disponibles :");
                compteur = 1;
                foreach (string value in recetteDisponible)
                {
                    Console.WriteLine(compteur + ". " + value);
                    compteur++;
                }
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();
                // On rentre la recette voulu
                Console.Write("\nIndiquer le nom de la recette que vous voulez commander > ");
                nomRecette = Console.ReadLine();
                nomRecetteValide = NomProduitValideListe(nomRecette, recetteDisponible);
                // Tant que la recette n'est pas correct, on demande au client de continuer d'ecrire
                while (nomRecetteValide == false)
                {
                    Console.WriteLine("\nReponse incorrect");
                    Console.Write("\nIndiquer le nom de la recette que vous voulez commander > ");
                    nomRecette = Console.ReadLine();
                    nomRecetteValide = NomProduitValideListe(nomRecette, recetteDisponible);
                }
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();
                // On rentre le nombre de la recette si dessus voulu
                Console.Write("\nIndiquer la quantité de la recette que vous voulez commander (entre 1 et 5) > ");
                nbRecette = Convert.ToInt32(Console.ReadLine());
                nbRecetteValide = NbRecetteValide(nbRecette);
                while (nbRecetteValide == false)
                {
                    Console.WriteLine("\nReponse incorrect");
                    Console.Write("\nIndiquer la quantité de la recette que vous voulez commander (entre 1 et 5) > ");
                    nbRecette = Convert.ToInt32(Console.ReadLine());
                    nbRecetteValide = NbRecetteValide(nbRecette);
                }
                // Une fois qu'on a récupérer un nom et une quantité de recette valide, on les stock
                listeNomRecetteCommande.Add(nomRecette);
                listeNbRecetteCommande.Add(nbRecette);
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();
                Console.Write("\nVoulez vous continuer a commander des recettes ? ");
                Console.Write("\nTapez 1 pour oui, 2 pour non > ");
                reponsecontinuer = Convert.ToInt32(Console.ReadLine());
                if (reponsecontinuer == 2)
                {
                    // Permet de quitter la boucle
                    continuer++;
                }
            }
            // On possède maintenant le nombre et nom de chacune des recettes que le client veut commander
            // ---------------------------Vérifier qu'il y a assez de produit une fois les recettes choisie---------------------------------------------

            // On créer d'abord une liste qui stock le numéro des recettes commandées (dans le meme ordre que la liste qui stock les noms des recettes)
            List<int> listeNumRecetteCommande = new List<int>(); //-------------------------------------------------------------------------------------//l1 (int)
            List<string> temporaire = new List<string>();
            foreach (string value in listeNomRecetteCommande)
            {
                temporaire = ConnexionBDDUneColonne("select numeroRecette from recette where nomRecette = '" + value + "';");
                listeNumRecetteCommande.Add(Convert.ToInt32(temporaire[0]));
            }

            // On possède deja une liste avec les quantité de recettes qui est : listeNbRecetteCommande ----------------------------------------//l1' (int)

            // On créer une liste qui va stocker les nom des produits (a partir de la table Utilise), on trie par nomProduit pour avoir un ordre
            List<string> listeNomProduitUtilise = ConnexionBDDUneColonne("select nomProduit from utilise order by nomProduit;"); //---------------------//l2' (string)

            // On créer une liste qui va stocker les numéro de recette (a partir de la table Utilise), dans le meme ordre que la liste du dessus
            List<int> listeNumRecetteUtilise = new List<int>(); //--------------------------------------------------------------------------------------//l2'' (int)
            List<string> temporaire2 = ConnexionBDDUneColonne("select numeroRecette from utilise order by nomProduit;");
            // On convertit en int, pour pouvoir le manipuler plus tard
            foreach (string num in temporaire2)
            {
                listeNumRecetteUtilise.Add(Convert.ToInt32(num));
            }

            // On créer une liste qui va stocker les quantités de produit utilisé par les recettes (a partir de la table utilise), 
            // dans le meme ordre que les 2 liste du dessus
            List<int> listeQuantitéUtilise = new List<int>(); //----------------------------------------------------------------------------------------//l2''' (int)
            List<string> temporaire3 = ConnexionBDDUneColonne("select quantiteProduits from utilise order by nomProduit;");
            // on convertit en int, pour pouvoir le manipuler plus tard 
            foreach (string qte in temporaire3)
            {
                listeQuantitéUtilise.Add(Convert.ToInt32(qte));
            }

            // Le but maintenant est de créer une liste qui va stocker le nom des produits a enlever (d'apres les recettes) 
            // et une autre liste qui stockera la quantité ses produits a enlever
            List<string> listeNomProduitEnlever = new List<string>(); //--------------------------------------------------------------------------------//l3 (string)
            List<int> listeQuantiteProduitEnlever = new List<int>(); //---------------------------------------------------------------------------------//l3' (int)
            // On va maintenant remplir ses deux listes:
            // On parcour d'abord la liste qui stock le numero des recettes (de la table Utilise) (l2'')
            for (int i = 0; i < listeNumRecetteUtilise.Count; i++)
            {
                // On parcours ensuite la liste qui stock les numeros des recettes commandé (l1)
                for (int j = 0; j < listeNumRecetteCommande.Count; j++)
                {
                    // On regarde quand le numéro des recettes est le meme (l2'') = (l1)
                    if (listeNumRecetteUtilise[i] == listeNumRecetteCommande[j])
                    {
                        // On incrémente la liste qui stock le nom du produit[i] a enlever (peut y avoir des doublons c'est normal) a l aide de (l2')
                        listeNomProduitEnlever.Add(listeNomProduitUtilise[i]);
                        // On incrémente la liste qui stock la quantité du produit[i] a enlever avec la formule :
                        //qté à enlever = qté utilisé dans la recette * qté de la recette commandé
                        //              = (l2''')*(l1')
                        listeQuantiteProduitEnlever.Add(listeQuantitéUtilise[i] * listeNbRecetteCommande[j]);
                    }
                }
            }
            // On affiche les resultats (pas neccessaire, mais permet de bien comprendre comment ca fonctionnne, ce que ca stock)
            Console.WriteLine("");
            for (int i = 0; i < listeQuantiteProduitEnlever.Count; i++)
            {
                Console.WriteLine("1.Enlever le produit : " + listeNomProduitEnlever[i] + " de : " + listeQuantiteProduitEnlever[i]);
            }
            // But créer une liste sans les doublons dans les nom des produit:
            // donc on va reprendre (l3) et (l3') pour qu'il n'y est plus de doublons
            List<string> listeNomProduitEnlever2 = new List<string>();// -----------------------------------------------------------------------------//l4 (string)
            List<int> listeQuantiteProduitEnlever2 = new List<int>(); //------------------------------------------------------------------------------//l4' (int)
            List<string> listeNomDejaUtilise = new List<string>();
            int compteurTrue = 0;
            int index = 0;
            for (int i = 0; i < listeNomProduitEnlever.Count; i++)
            {
                //Pour initialiser les listes
                if (i == 0)
                {
                    listeNomProduitEnlever2.Add(listeNomProduitEnlever[i]);
                    listeQuantiteProduitEnlever2.Add(listeQuantiteProduitEnlever[i]);
                    listeNomDejaUtilise.Add(listeNomProduitEnlever[i]);
                }
                else
                {
                    compteurTrue = 0;
                    // On parcour la liste des nom de produit deja utilisé
                    foreach (string value in listeNomDejaUtilise)
                    {
                        // si le produit est deja dans la liste
                        if (value == listeNomProduitEnlever[i])
                        {
                            compteurTrue++;
                        }
                    }
                    // si le produit est deja dans la liste, on chercher son index dans (l4),
                    // et on incremente la quantité (l3') dans (l4') a l'index deduit
                    if (compteurTrue >= 1)
                    {
                        index = listeNomProduitEnlever2.IndexOf(listeNomProduitEnlever[i]);
                        listeQuantiteProduitEnlever2[index] += listeQuantiteProduitEnlever[i];
                    }
                    // sinon, on rajoute le nom du produit (l3) dans la liste (l4) et listeNomDejaUtilise
                    // et aussi on rajoute la quantité (l3') dans la liste (l4')
                    else
                    {
                        listeNomProduitEnlever2.Add(listeNomProduitEnlever[i]);
                        listeQuantiteProduitEnlever2.Add(listeQuantiteProduitEnlever[i]);
                        listeNomDejaUtilise.Add(listeNomProduitEnlever[i]);
                    }
                }
            }
            // On affiche les resultats (pas neccessaire, mais permet de bien comprendre comment ca fonctionnne, ce que ca stock)
            Console.WriteLine("");
            for (int i = 0; i < listeNomProduitEnlever2.Count; i++)
            {
                Console.WriteLine("2.Enlever le produit : " + listeNomProduitEnlever2[i] + " de : " + listeQuantiteProduitEnlever2[i]);
            }

            // On créer une liste qui stock tous les nomProduits de la BDD a partir de produit: (dans un ordre precis)
            List<string> listeNomProduitProduit = ConnexionBDDUneColonne("select nomProduit from produit order by nomProduit;"); //--------------------//l5 (string)

            // On créer une liste qui va stocker tous les stocks actuel des produits a partir de produit
            // (dans le meme ordre que la liste si dessus)
            List<string> temporaire4 = ConnexionBDDUneColonne("select stockActuel from produit order by nomProduit;");
            List<int> listeStockActuelProduit = new List<int>(); //------------------------------------------------------------------------------------//l5' (int)
            foreach (string stock in temporaire4)
            {
                listeStockActuelProduit.Add(Convert.ToInt32(stock));
            }

            // But créer une liste qui va stocker le stock de chaque produit si la commande est validé:
            List<int> listeStockFinale = new List<int>(); //------------------------------------------------------------------------------------------//l6 (int)
            // On va maintenant remplir cette liste:
            // On parcours la liste de tous les nom des produit (a partir de Produit) (l5)
            for (int i = 0; i < listeNomProduitProduit.Count; i++)
            {
                // On parcours la liste des produits a enlever (l4)
                for (int j = 0; j < listeNomProduitEnlever2.Count; j++)
                {
                    // Si le nomProduit[i](l5) == nomProduitEnlever2[j](l4), alors on fait stock actuel(l5') - stock a enlever(l4')
                    if (listeNomProduitProduit[i] == listeNomProduitEnlever2[j])
                    {
                        listeStockFinale.Add(listeStockActuelProduit[i] - listeQuantiteProduitEnlever2[j]);
                    }
                }
            }
            // On affiche les resultats (pas neccessaire, mais permet de bien comprendre comment ca fonctionnne, ce que ca stock)
            Console.WriteLine("");
            for (int i = 0; i < listeStockFinale.Count; i++)
            {
                Console.WriteLine("Le stock final de : " + listeNomProduitEnlever2[i] + " est de : " + listeStockFinale[i]);
            }

            // Pour savoir si on peut passer la commande on va voir si tous les stocks de l6 sont positifs:
            int compteur2 = 0;
            // On parcours les stock finaux
            foreach (int stock in listeStockFinale)
            {
                // Si un des stocks est inferieur strict a 0 (donc negatif)
                if (stock < 0)
                {
                    compteur2++;
                }
            }
            // Si le compteur est >= a 1, alors l'un des stock est négatif et on ne peut pas passer la commande
            if (compteur2 >= 1)
            {
                Console.WriteLine("Il n'y a pas assez de stock pour effectuer la commande voulue, veuillez nous excuser");
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();
            }
            // ----------Si le compteur est inférieur a 1, alors tous les stocks sont positifs apres passage de commande, on peut donc continuer la demarche!--------
            else
            {
                Console.WriteLine("\nIl y a assez de stock pour passer la commande, nous allons maintenant procéder au paiement de la commande :");
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();

                // But : calculer le prix total de la commande a payer, avant de la valider
                // On va créer une liste qui stock les prix de chacune des recettes commmandés
                List<int> listePrixRecetteUnitaire = new List<int>(); //-----------------------------------------------------------------------------//L7 (int)
                List<string> temporaire5 = new List<string>();
                // On remplit la liste avec le prix des recettes commandé, en effectuant la connexion on aura seulement des strings
                // aussi cette liste va stocker le prix de chaque recette (mais uniquement son prix unitaire)
                foreach (int numRecette in listeNumRecetteCommande)
                {
                    temporaire5 = ConnexionBDDUneColonne("select prixVente from recette where numeroRecette = " + numRecette + ";");
                    listePrixRecetteUnitaire.Add(Convert.ToInt32(temporaire5[0]));
                }
                //-------------------------------------On calcul le prix total a payer-------------------------------------------------------------------------------
                int prixTotale = 0;
                for (int i = 0; i < listePrixRecetteUnitaire.Count; i++)
                {
                    //prix totale += prix de la recette commandé (l7)* quantité de la recette commandé (l1')
                    prixTotale += listePrixRecetteUnitaire[i] * listeNbRecetteCommande[i];
                }

                // On affiche le prix a payer:
                Console.WriteLine("\nLe prix total de votre commande a payer est de : " + prixTotale + " cooks.");
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();

                //-----------------------------------But : obtenir le solde cook du client :-------------------------------------------------------------------------
                List<string> soldeClientString = ConnexionBDDUneColonne("select creditCook from client where pseudo = '" + identifiant + "';");
                int creditCook = Convert.ToInt32(soldeClientString[0]);
                Console.WriteLine("\nVotre solde actuel est de  : " + creditCook + " cooks.");
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();

                // On regarde si le solde du client est suffisant, si non, on rajoute 20 a son solde jusqu'a ce qu'il soit suffisant pour payer la commande
                while (creditCook < prixTotale)
                {
                    Console.WriteLine("\nVotre solde actuelle est insufisant, nous allons vous rajouter 20 cooks.");
                    creditCook += 20;
                    Connexion_BDD2("update client set creditCook = " + creditCook + " where pseudo = '" + identifiant + "';");
                    Console.WriteLine("Appuyer sur une touche pour continuer");
                    Console.ReadKey();
                    Console.WriteLine("\nVotre solde actuelle est de : " + creditCook + " cooks.");
                    Console.WriteLine("Appuyer sur une touche pour continuer");
                    Console.ReadKey();
                }
                Console.WriteLine("\nVotre solde actuel est suffisant nous allons proceder a la validation de votre commande.");
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();

                // -------------------------------Création de la commande dans la base de donnée----------------------------------------------------------------------
                // On cherche d'abord le numéro de commande
                // On créer une liste qui stock les numéro de commande qui existe deja, ordonner du plus grand au plus petit
                List<string> numeroDeCommandeMax = ConnexionBDDUneColonne("select numeroCommande from commande order by numeroCommande Desc;");

                // On stock le numero de la nouvelle commande par = numero de commande le plus grand + 1
                int numeroCommande = Convert.ToInt32(numeroDeCommandeMax[0]) + 1;

                // On stock la date d'aujourd'hui dans le bon format (string de la forme YYYY-MM-JJ)
                DateTime thisDate = DateTime.Now;
                CultureInfo culture = new CultureInfo("ja-JP");
                string date = thisDate.ToString("d", culture);
                string dateFinale = "";
                for (int i = 0; i < date.Length; i++)
                {
                    if (i == 4 || i == 7)
                    {
                        dateFinale += "-";
                    }
                    else
                    {
                        dateFinale += date[i];
                    }
                }

                // On possede maintenant toutes les données pour insérer la commande dans la BDD
                Connexion_BDD2("insert into `cooking`.`commande` (`numeroCommande`, `dateCommande`, `prixTotal`, `pseudo`) VALUES (" + numeroCommande + ", '" + dateFinale + "' ," + prixTotale + ", '" + identifiant + "');");
                Console.WriteLine("Commande passé");

                // -------------------------------------------Il faut créer les tuple contients :--------------------------------------------------------------------
                // Nous allons utilisé l1 et l1' afin de remplir la bdd
                for (int i = 0; i < listeNumRecetteCommande.Count; i++)
                {
                    Connexion_BDD2("INSERT INTO `cooking`.`contient` (`numeroCommande`, `numeroRecette`, `quantiteRecette`) VALUES (" + numeroCommande + ", " + listeNumRecetteCommande[i] + ", " + listeNbRecetteCommande[i] + ");");
                }
                Console.WriteLine("Tuple Contient créé ");

                //--------------------------------------On va maintenant deduire le solde cook du compte client-------------------------------------------------------
                int soldeCookApresCommande = creditCook - prixTotale;
                // On update le creditCook du client concerné
                Connexion_BDD2("update client set creditCook = " + soldeCookApresCommande + " where pseudo = '" + identifiant + "';");
                Console.WriteLine("Update creditCook client done");


                //---------------------------------------Il faut maintenant rémunérer les cdr concernés---------------------------------------------------------------
                // On créer d'abord une liste qui va stocker les pseudos qu'il faut remunérer pour chaque recette:
                List<string> listePseudoARemunerer = new List<string>(); //------------------------------------------------------------------------------//L8 (string)
                List<string> temporaire6 = new List<string>();
                // On remplit la liste avec le pseudo des recettes commandé
                foreach (int numRecette in listeNumRecetteCommande)
                {
                    temporaire6 = ConnexionBDDUneColonne("select pseudo from recette where numeroRecette = " + numRecette + ";");
                    listePseudoARemunerer.Add(temporaire6[0]);
                }
                Console.WriteLine("listePseudoARemunerer créée");

                // On créer la liste des soldes actuelles des cdr concernés par la commande
                List<int> listeSoldeActuel = new List<int>(); //-----------------------------------------------------------------------------//L8' (int)
                List<string> temporaire7 = new List<string>();
                // On remplit la liste avec le creditCook des pseudo des recettes commandé
                foreach (string pseudo in listePseudoARemunerer)
                {
                    temporaire7 = ConnexionBDDUneColonne("select C.creditCook from recette R, client C where R.pseudo = '" + pseudo + "' and C.pseudo = R.pseudo;");
                    listeSoldeActuel.Add(Convert.ToInt32(temporaire7[0]));
                }
                Console.WriteLine("listeSoldeActuel créée ");

                // On créer la liste qui va stocker les Remuneration Cdr de chaque recette:
                List<int> listeQuantiteARemunerer = new List<int>(); //-----------------------------------------------------------------------------//L8'' (int)
                List<string> temporaires = new List<string>();
                for (int i = 0; i < listeNumRecetteCommande.Count; i++)
                {
                    temporaires = ConnexionBDDUneColonne("select remunerationCdR from recette where numeroRecette = " + listeNumRecetteCommande[i] + ";");
                    listeQuantiteARemunerer.Add(Convert.ToInt32(temporaires[0]) * listeNbRecetteCommande[i]);
                }
                Console.WriteLine("listeQuantiteARemunerer créée");

                // Il faut maintenant crediter les pseudo l8 de la somme l8'' en remplacant le creditCook actuel par l8' + l8''
                int quantiteFinaleCook = 0;
                for (int i = 0; i < listePseudoARemunerer.Count; i++)
                {
                    quantiteFinaleCook = listeQuantiteARemunerer[i] + listeSoldeActuel[i];
                    Connexion_BDD2("update client set creditCook = " + quantiteFinaleCook + " where pseudo = '" + listePseudoARemunerer[i] + "';");
                }
                Console.WriteLine("Update crediCook CdR done");

                //-----------------------------------Il faut maintenant incrémenter les compteur des recettes commandés:---------------------------------------------
                //Pour ca on va utilisé la liste l1 et l1' et créer une liste qui stock les compteurs actuels des recettes concernés

                // On créer la liste des compteurs actuels des recettes concernés
                List<int> listeCompteurActuel = new List<int>(); //----------------------------------------------------------------------------------------//L9 (int)
                List<string> temporaire8 = new List<string>();
                // On remplit la liste avec le compteur des recettes commandé
                foreach (int numRecette in listeNumRecetteCommande)
                {
                    temporaire8 = ConnexionBDDUneColonne("select compteur from recette where numeroRecette = " + numRecette + ";");
                    listeCompteurActuel.Add(Convert.ToInt32(temporaire8[0]));
                }
                Console.WriteLine("listeCompteurActuel créée");

                // On va maintenant update les compteurs de chaque recette:
                // avec la formule compteur finale = compteur actuelle(l9) + quantité recette commmandé(l1')
                int quantiteFinaleCompteur = 0;
                for (int i = 0; i < listeNumRecetteCommande.Count; i++)
                {
                    quantiteFinaleCompteur = listeCompteurActuel[i] + listeNbRecetteCommande[i];
                    Connexion_BDD2("update recette set compteur = " + quantiteFinaleCompteur + " where numeroRecette = " + listeNumRecetteCommande[i] + ";");
                }
                Console.WriteLine("Update compteur recette done");

                // ----------------------On doit vérifier si le compteur depasse les 10,  si oui, le prix de la recette augmente de 2 cook---------------------------
                // On doit créer une liste qui stock les compteur recettes, apres avoir été mis a jour
                List<int> listeCompteurApres = new List<int>(); //-----------------------------------------------------------------------------------------//L10 (int)
                List<string> temporaire9 = new List<string>();

                // On remplit la liste avec le compteur des recettes commandé
                foreach (int numRecette in listeNumRecetteCommande)
                {
                    temporaire9 = ConnexionBDDUneColonne("select compteur from recette where numeroRecette = " + numRecette + ";");
                    listeCompteurApres.Add(Convert.ToInt32(temporaire9[0]));
                }
                Console.WriteLine("listeCompteurApres créée");

                // On possède deja la liste des prix de vente actuelle des recettes avec l7
                // On creer une nouvelle liste qui va stocker les prix de vente finaux
                List<int> listePrixVenteApres = new List<int>(); //-----------------------------------------------------------------------------------------//L11 (in)
                int prixVenteApres = 0;
                //On parcours la liste des compteur:
                for (int i = 0; i < listeCompteurApres.Count; i++)
                {
                    if (listeCompteurApres[i] >= 10)
                    {
                        prixVenteApres = listePrixRecetteUnitaire[i] + 2;
                    }
                    if (listeCompteurApres[i] >= 10)
                    {
                        prixVenteApres = listePrixRecetteUnitaire[i] + 5;
                    }
                    else
                    {
                        //ca ne change pas
                        prixVenteApres = listePrixRecetteUnitaire[i];
                    }
                    Connexion_BDD2("update recette set prixVente = " + prixVenteApres + " where numeroRecette = " + listeNumRecetteCommande[i] + " ;");
                }
                Console.WriteLine("Update prixVente done");

                // -------------------------------------Enfin on doit décrémenter les stocks des produits utilisé:---------------------------------------------------
                // On va utilisé la liste qui stock le stock de chaque produit si la commande est validé: (l6)
                // On parcours la liste de tous les nom des produit (a partir de Produit)(l5)
                for (int i = 0; i < listeNomProduitProduit.Count; i++)
                {
                    // On parcours la liste des produits a enlever (l4)
                    for (int j = 0; j < listeNomProduitEnlever2.Count; j++)
                    {
                        // Si le nomProduit[i](l5) == nomProduitEnlever2[j](l4), alors on fait stock actuel(l5') - stock a enlever(l4')
                        if (listeNomProduitProduit[i] == listeNomProduitEnlever2[j])
                        {
                            Connexion_BDD2("update produit set stockActuel = " + listeStockFinale[j] + " where nomProduit = '" + listeNomProduitProduit[i] + "';");
                        }
                    }
                }
                Console.WriteLine("Update stock done");
            }
        }

        //---------------------------------Permet de créer une recette----------------------------------------------

        /// <summary>
        /// Permet de determiner si le nom de la recette passé en parametre est valide
        /// cad qu'il fait moins de 30 caractères et ne contient pas de ' et de `
        /// </summary>
        /// <param name="newNomRecette">le nom de la recette</param>
        /// <returns>vraie si correct, faux sinon</returns>
        static bool NewNomRecetteValide(string newNomRecette)
        {
            bool reponse = false;
            if (newNomRecette.Length < 30 && newNomRecette.Contains("'") == false && newNomRecette.Contains("`") == false)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de determiner si le type de la recette passé en parametre est valide
        /// cad qu'il fait moins de 20 caractères et ne contient pas de ' et de `
        /// </summary>
        /// <param name="newTypeRecette">le type de la recette</param>
        /// <returns>vraie si correct, faux sinon</returns>
        static bool NewTypeRecetteValide(string newTypeRecette)
        {
            bool reponse = false;
            if (newTypeRecette.Length < 20 && newTypeRecette.Contains("'") == false && newTypeRecette.Contains("`") == false)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        ///  Permet de determiner si le type de la recette passé en parametre est valide
        /// cad qu'il fait moins de 150 caractères et ne contient pas de ' et de `
        /// </summary>
        /// <param name="newDesRecette">descriptif de la recette</param>
        /// <returns>vraie si correct, faux sinon</returns>
        static bool NewDesRecetteValide(string newDesRecette)
        {
            bool reponse = false;
            if (newDesRecette.Length < 150 && newDesRecette.Contains("'") == false && newDesRecette.Contains("`") == false)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// permet de savoir si le prix est compris entre 10 et 40 cook
        /// </summary>
        /// <param name="newPrixRecette">prix</param>
        /// <returns>vraie si dans les bornes, faux sinon</returns>
        static bool NewPrixRecetteValide(int newPrixRecette)
        {
            bool reponse = false;
            if (newPrixRecette >= 10 && newPrixRecette <= 40)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de savoir si le nom du produit passe en parametre est present dans la liste passe en parametre
        /// Utilisé pour produit mais aussi pour recette (dans création de commande)
        /// </summary>
        /// <param name="nomProduit">nom du produit</param>
        /// <param name="listeProduit">liste</param>
        /// <returns>vraie si presnet dans la liste, faux sinon</returns>
        static bool NomProduitValideListe(string nomProduit, List<string> listeProduit)
        {
            bool reponse = false;
            int compteur = 0;
            foreach (string values in listeProduit)
            {
                if (values == nomProduit)
                {
                    compteur++;
                }
            }
            if (compteur == 1)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de verifier que le chiffre rentré en parametre est bien compris entre 1 et 5
        /// </summary>
        /// <param name="quantiteProduit">la quantité de produit</param>
        /// <returns>vraie si valide, faux sinon</returns>
        static bool QuantiteProduitValide(int quantiteProduit)
        {
            bool reponse = false;
            if (quantiteProduit >= 1 && quantiteProduit <= 5)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// permet de créer un tuple utilise a l'aide des données passées en paramètre
        /// </summary>
        /// <param name="nomProduit">nom du produit</param>
        /// <param name="numeroRecette">numero de la recette</param>
        /// <param name="quantiteProduit">quantite du produit</param>
        static void InsererUtilise(string nomProduit, int numeroRecette, int quantiteProduit)
        {
            Connexion_BDD2("INSERT INTO cooking.`utilise` (`nomProduit`,`numeroRecette`,`quantiteProduits`) VALUES ('" + nomProduit + "', '" + numeroRecette + "', '" + quantiteProduit + "'); ");
        }

        /// <summary>
        /// Permet de creer un tuple recette a l'aide des données passé en parametre
        /// </summary>
        /// <param name="numeroRecette">numero de la recette</param>
        /// <param name="newNomRecette">nom de la recette</param>
        /// <param name="newTypeRecette">type de la recette</param>
        /// <param name="newDesRecette">description de la recette</param>
        /// <param name="newPrixRecette">prix de la recette</param>
        /// <param name="newRemuneration">remuneration du cdr de la recette</param>
        /// <param name="newCompteur">compteur de la recette</param>
        /// <param name="identifiant">pseudo du cdr</param>
        static void InsererRecette(int numeroRecette, string newNomRecette, string newTypeRecette, string newDesRecette, int newPrixRecette, int newRemuneration, int newCompteur, string identifiant)
        {
            Connexion_BDD2("INSERT INTO cooking.`recette` (`numeroRecette`,`nomRecette`,`typeRecette`,`descriptif`,`prixVente`,`remunerationCdR`,`compteur`, `pseudo`) VALUES (" + numeroRecette + ", '" + newNomRecette + "', '" + newTypeRecette + "', '" + newDesRecette + "', " + newPrixRecette + ", " + newRemuneration + ", " + newCompteur + ", '" + identifiant + "'); ");
        }

        /// <summary>
        /// Permet au client rentré en parametre de créer une recette
        /// Utilisé quand un client s'est identifier comme Cdr et veut créer une recette
        /// </summary>
        /// <param name="identifiant">pseudo du Cdr</param>
        static void CreerRecette(string identifiant)
        {
            //Determiner le numero de la recette:
            //On recupere tous les numero des recette du plus grand au plus petit (au cas ou une recette est supprimé)
            // et le nouveau numéro de recette sera = numero le plus grand + 1
            List<string> nbRecette = ConnexionBDDUneColonne("select numeroRecette from recette order by numeroRecette DESC;");
            int numeroRecette = Convert.ToInt32(nbRecette[0]) + 1;

            // On demande de créer un nom de recette
            Console.Write("\nVeuillez rentrer le nom de votre recette (moins de 30 caractères) > ");
            string newNomRecette = Console.ReadLine();
            bool NewNomRecetteCorrect = NewNomRecetteValide(newNomRecette);
            // on vérifie que le nom est bon
            while (NewNomRecetteCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer le nom de votre recette (moins de 30 caractères) > ");
                newNomRecette = Console.ReadLine();
                NewNomRecetteCorrect = NewNomRecetteValide(newNomRecette);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            // On demande de créer un type de recette
            Console.Write("\nVeuillez rentrer le type de votre recette (moins de 20 caractères) > ");
            string newTypeRecette = Console.ReadLine();
            bool NewTypeRecetteCorrect = NewTypeRecetteValide(newTypeRecette);
            // on verifie u-qu'il est bon
            while (NewTypeRecetteCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer le type de votre recette (moins de 20 caractères) > ");
                newTypeRecette = Console.ReadLine();
                NewTypeRecetteCorrect = NewTypeRecetteValide(newTypeRecette);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            // On demande de rentrer un descriptif de la recette
            Console.Write("\nVeuillez rentrer le descriptif de votre recette (moins de 150 caractères) > ");
            string newDesRecette = Console.ReadLine();
            bool NewDesRecetteCorrect = NewDesRecetteValide(newDesRecette);
            // on verifie qu'il est bon
            while (NewTypeRecetteCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer le descriptif de votre recette (moins de 150 caractères) > ");
                newDesRecette = Console.ReadLine();
                NewDesRecetteCorrect = NewDesRecetteValide(newDesRecette);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            // On demande le prix de vente de la recette:
            Console.Write("\nVeuillez rentrer le prix de vente de votre recette entre 10 et 40 cook > ");
            int newPrixRecette = Convert.ToInt32(Console.ReadLine());
            bool NewPrixRecetteCorrect = NewPrixRecetteValide(newPrixRecette);
            // on verifie qu'il est bon
            while (NewPrixRecetteCorrect == false)
            {
                Console.WriteLine("\nLa réponse est incorrecte");
                Console.Write("Veuillez rentrer le descriptif de votre recette (moins de 150 caractères) > ");
                newPrixRecette = Convert.ToInt32(Console.ReadLine());
                NewPrixRecetteCorrect = NewPrixRecetteValide(newPrixRecette);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            // On fixe la remunération du CdR a deux
            int newRemuneration = 2;

            // On fixe le compteur de commande a 0 (car forcement sa recette n'a jamais ete commandé
            int newCompteur = 0;

            // On possède deja le pseudo 

            //IL FAUT CREER LA RECETTE DANS LA BDD SINON ON NE PEUT PAS FAIRE LES UTILISES
            // On insère la recette dans la BDD, on sait deja que toutes les données sont corrects
            InsererRecette(numeroRecette, newNomRecette, newTypeRecette, newDesRecette, newPrixRecette, newRemuneration, newCompteur, identifiant);

            // Maintenant il faut lui demander quels sont les produits qui constituent sa recette
            // On lui affiche une liste de produit qu'il peut prendre pour constituer sa recette:
            Console.WriteLine("Voici la liste des produit avec laquelle vous pouvez consituer votre recette :");
            List<string> listeProduit = ConnexionBDDUneColonne("select nomProduit from produit;");
            int compteur2 = 1;
            foreach (string value in listeProduit)
            {
                Console.WriteLine(compteur2 + ". " + value);
                compteur2++;
            }
            Console.Write("\nVeuillez d'abord taper le nombre de produit parmis la liste si dessus qui vous interresse > ");
            int nbProduit = Convert.ToInt32(Console.ReadLine());
            // On initialise les variables
            List<string> listeNomProduit = new List<string>(nbProduit);
            List<int> listeQuantitéProduit = new List<int>(nbProduit);
            string nomProduit = "";
            int quantiteProduit = 0;
            bool nomProduitCorrect = false;
            bool quantiteProduitCorrect = false;
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            Console.WriteLine("\nNous allons maintenant vous demandez quelles sont les produits qui vous interresse ainsi que leur quantité");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            for (int i = 0; i < nbProduit; i++)
            {
                // On demande le nom du produit
                Console.Write("\nVeuillez ecrire le produit qui vous interresse > ");
                nomProduit = Console.ReadLine();
                nomProduitCorrect = NomProduitValideListe(nomProduit, listeProduit);
                while (nomProduitCorrect == false)
                {
                    Console.WriteLine("\nRéponse incorrect");
                    Console.Write("\nVeuillez ecrire le produit qui vous interresse > ");
                    nomProduit = Console.ReadLine();
                    nomProduitCorrect = NomProduitValideListe(nomProduit, listeProduit);
                }
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();

                // On demande la quantité du produit
                Console.Write("\nVeuillez ecrire la quantité de ce produit qui vous interresse (entre 1 et 5) > ");
                quantiteProduit = Convert.ToInt32(Console.ReadLine());
                quantiteProduitCorrect = QuantiteProduitValide(quantiteProduit);
                while (nomProduitCorrect == false)
                {
                    Console.WriteLine("\nRéponse incorrect");
                    Console.Write("\nVeuillez ecrire la quantité de produit qui vous interresse (entre 1 et 5) > ");
                    quantiteProduit = Convert.ToInt32(Console.ReadLine());
                    quantiteProduitCorrect = QuantiteProduitValide(quantiteProduit);
                }
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();

                // On créer le tuple "utilise" avec ses données
                InsererUtilise(nomProduit, numeroRecette, quantiteProduit);
            }
            Console.WriteLine("Recette créer avec succès !");
        }

        /// <summary>
        /// Permet au client passé en parametre de pouvoir afficher son solde cook
        /// Utilisé quand un client s'est identifié comme un Cdr et il veut consulter son solde cook
        /// </summary>
        /// <param name="identifiant">pseudo du client</param>
        static void ConsulterLeSolde(string identifiant)
        {
            List<string> solde = ConnexionBDDUneColonne("select creditCook from client where pseudo = " + '\u0022' + identifiant + '\u0022' + ";");
            Console.WriteLine("Votre solde est de " + solde[0] + " cook");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche la liste des recettes d'un CdR
        /// Utilisé quand un client s'est identifié comme cdr et veut avoir accès ses recettes
        /// </summary>
        /// <param name="identifiant"></param>
        static void AfficherListeRecetteCdr(string identifiant)
        {
            Console.WriteLine("\nVous allez maintenant voir toutes vos recettes ainsi que leur nombre de commande");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            Console.WriteLine("");
            List<string> nomRecetteNbRecette = ConnexionBDDDeuxColonne("select R.nomRecette, R.compteur from recette R where R.Pseudo = " + '\u0022' + identifiant + '\u0022' + ";");
            if (nomRecetteNbRecette.Count == 0)
            {
                Console.WriteLine("Vous n'avez pas encore créer de recette");
            }
            for (int i = 0; i < nomRecetteNbRecette.Count; i += 2)
            {
                Console.WriteLine("La recette : " + nomRecetteNbRecette[i] + " a un nombre de commande de : " + nomRecetteNbRecette[i + 1]);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
        }

        //----------------------------------------------------------------------------------


        //---------------------Utilisé dans gestionnaire cooking---------------------------- Noël


        /// <summary>
        /// fonction qui parcourt toutes les dates des commandes passées pour un produit en particulier, récupère la dernière date,
        /// vérifie si cette date est dépassée de plus de 30 jours, si oui, les quantités max et min sont divisées par 2, et elle sont mises à jour
        /// dans la base de donnée
        /// </summary>
        static void MiseAJour(string aliment)
        {
            List<string> listeDates = ConnexionBDDUneColonne("select Comm.dateCommande from Commande Comm, Contient C, Recette R, Utilise U, Produit  P " +
                "where Comm.numeroCommande = C.numeroCommande and C.numeroRecette = R.numeroRecette and U.numeroRecette = R.numeroRecette" +
                " and U.nomProduit = P.nomProduit and P.nomProduit = '" + aliment + "' order by dateCommande DESC; ");
            string dateDernièreCommande = "";
            string dateRaccourcie = "";
            DateTime dateConvertie = new DateTime();
            DateTime dateFuture = new DateTime();
            string format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime dateToday = DateTime.Now;
            if (listeDates.Count != 0)
            {
                Console.WriteLine("\nListe des dates des commandes contenant l'aliment " + aliment);
                for (int i = 0; i < listeDates.Count; i++)
                {
                    Console.WriteLine(listeDates[i]);
                }
                dateDernièreCommande = listeDates[0];
                dateRaccourcie = dateDernièreCommande.Substring(0, 10);
                dateConvertie = DateTime.ParseExact(dateRaccourcie, format, provider);
                dateFuture = dateConvertie.AddDays(30);
                Console.WriteLine("\nLa dernière commande a eu lieu le " + dateRaccourcie);
                Console.WriteLine("\nEt nous sommes le " + dateToday);
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ce produit ne fait partie d'aucune commande du dernier mois\n");
                Console.ReadKey();
            }
            List<string> listeStocks = ConnexionBDDDeuxColonne("select stockMinimal, stockMaximal from produit where nomProduit ='" + aliment + "';");
            if (dateFuture > dateToday)
            {
                Console.WriteLine("\nLes quantités pour l'aliment " + aliment + " sont à jour.");
                Console.WriteLine("Stock minimal : " + listeStocks[0]);
                Console.WriteLine("Stock maximal : " + listeStocks[1]);
                Console.WriteLine("\nAppuyez sur une touche pour continuer\n");
                Console.ReadKey();

            }
            if (dateFuture < dateToday)
            {
                Connexion_BDD2("update produit set stockMinimal = stockMinimal/2, stockMaximal = stockMaximal/2 where nomProduit = '" + aliment + "';");
                List<string> listeStocksAJour = ConnexionBDDDeuxColonne("select stockMinimal, stockMaximal from produit where nomProduit ='" + aliment + "';");
                Console.WriteLine("\nLes quantités pour l'aliment " + aliment + " viennent d'être mises à jour.");
                Console.WriteLine("Stock minimal : " + listeStocksAJour[0]);
                Console.WriteLine("Stock minimal : " + listeStocksAJour[1]);
                Console.WriteLine("\nAppuyez sur une touche pour continuer\n");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Permet de prendre une commande et de retourner une liste
        /// Utilisée ici quand la commande SQL affiche 5 colonnes mais que seul la colonne du numéro en question est retournée (pour garder l'ordre souhaité)
        /// Placera les elements de la ligne une cote a cote, puis la ligne 2 cote a cote, etc
        /// </summary>
        /// <param name="commande">commande a executer</param>
        /// <returns>liste composé des resultat de la commande</returns>
        static List<string> ConnexionBDDSpecialReappro(string commande, int numeroColonne)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Basket1204;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = commande;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            List<string> resultat = new List<string>();

            string mdp1;
            while (reader.Read())// parcours ligne par ligne
            {
                mdp1 = reader.GetString(numeroColonne);
                resultat.Add(mdp1);
            }
            connection.Close();
            return resultat;
        }

        /// <summary>
        /// fonction qui va retourner la liste des quantités min, ordonnée selon les consignes 
        /// fait appel à la fonction connexionBDDColonneQteMin pour avoir la liste ordonnée par nomFournisseur et nomProduit sans retourner
        /// les listes qu'on ne veut pas
        /// </summary>
        /// <returns>liste des quantités min</returns>
        static List<string> ListeQuantiteMinTriee()
        {
            List<string> listeQuantiteMin = ConnexionBDDSpecialReappro("select F.nomFournisseur, P.nomProduit, P.stockActuel, P.stockMinimal," +
                " P.stockMaximal from produit P, fournisseur F where F.nomFournisseur = P.nomFournisseur and stockActuel < stockMinimal order by nomfournisseur," +
                " nomProduit;", 3);
            return listeQuantiteMin;
        }

        /// <summary>
        /// fonction qui va retourner la liste des quantités max, ordonnée selon les consignes 
        /// fait appel à la fonction connexionBDDColonneQteMax pour avoir la liste ordonnée par nomFournisseur et nomProduit sans retourner
        /// les listes qu'on ne veut pas
        /// </summary>
        /// <returns>liste des quantités max</returns>
        static List<string> ListeQuantiteMaxTriee()
        {
            List<string> listeQuantiteMax = ConnexionBDDSpecialReappro("select F.nomFournisseur, P.nomProduit, P.stockActuel, P.stockMinimal," +
                " P.stockMaximal from produit P, fournisseur F where F.nomFournisseur = P.nomFournisseur and stockActuel < stockMinimal order by nomfournisseur," +
                " nomProduit;", 4);
            return listeQuantiteMax;
        }

        /// <summary>
        /// fonction qui va renvoyer la liste des quantités à commander 
        /// calculée à partir de la soustraction des deux fonctions précédentes, prises en paramètre
        /// </summary>
        /// <param name="listeQuantiteMin"></param>
        /// <param name="listeQuantiteMax"></param>
        /// <returns>liste Quantité A Commander</returns>
        static List<string> QuantiteACommander(List<string> listeQuantiteMin, List<string> listeQuantiteMax)
        {
            List<string> listeQuantiteACommander = new List<string>();
            for (int i = 0; i < listeQuantiteMin.Count; i++)
            {
                listeQuantiteACommander.Add(Convert.ToString(Convert.ToInt32(listeQuantiteMax[i]) - Convert.ToInt32(listeQuantiteMin[i])));
            }
            return listeQuantiteACommander;
        }

        /// <summary>
        /// fonction qui va retourner la liste des nomProduits, ordonnée selon les consignes 
        /// fait appel à la fonction connexionBDDColonneProduit pour avoir la liste ordonnée par nomFournisseur et nomProduit sans retourner
        /// les listes qu'on ne veut pas
        /// sera utilisée en paramètre de la fonction ListeCommandeFournisseur 
        /// </summary>
        /// <returns>liste des quantités min</returns>
        static List<string> ListeProduits()
        {
            List<string> listeProduits = ConnexionBDDSpecialReappro("select F.nomFournisseur, P.nomProduit, P.stockActuel, P.stockMinimal," +
                " P.stockMaximal from produit P, fournisseur F where F.nomFournisseur = P.nomFournisseur and stockActuel < stockMinimal order by nomfournisseur," +
                " nomProduit;", 1);
            return listeProduits;
        }

        /// <summary>
        /// fonction qui va retourner la liste des nomFournisseurs, ordonnée selon les consignes 
        /// fait appel à la fonction connexionBDDColonneFournisseur pour avoir la liste ordonnée par nomFournisseur et nomProduit sans retourner
        /// les listes qu'on ne veut pas
        /// sera utilisée en paramètre de la fonction ListeCommandeFournisseur 
        /// </summary>
        /// <returns>liste des quantités min</returns>
        static List<string> ListeFournisseurs()
        {
            List<string> listeFournisseur = ConnexionBDDSpecialReappro("select F.nomFournisseur, P.nomProduit, P.stockActuel, P.stockMinimal," +
                " P.stockMaximal from produit P, fournisseur F where F.nomFournisseur = P.nomFournisseur and stockActuel < stockMinimal order by nomfournisseur," +
                " nomProduit;", 0);
            return listeFournisseur;
        }

        /// <summary>
        /// fonction qui retourne la liste finale de commande qui sera sérialisée pour le fichier XML
        /// ce n'est qu'un assemblage de listes fournisseurs produits et quantité en une seule liste
        /// nécessité de renvoyer un list<string> et non un string[] car impossible de sérialiser un tableau, que des listes
        /// /// </summary>
        /// <param name="listeFournisseurs"></param>
        /// <param name="listeProduits"></param>
        /// <param name="listeQuantite"></param>
        /// <returns>liste finale commande</returns>
        static List<string> ListeCommandeFournisseur(List<string> listeFournisseurs, List<string> listeProduits, List<string> listeQuantite)
        {
            List<string> listeFinaleCommande = new List<string>();
            string[,] tab = new string[4, 3];
            for (int i = 0; i < listeFournisseurs.Count; i++)
            {
                for (int j = 0; j < 3; j += 3)
                {
                    tab[i, j] = (listeFournisseurs[i]);
                    tab[i, j + 1] = (listeProduits[i]);
                    tab[i, j + 2] = (listeQuantite[i]);
                }
            }
            for (int i = 0; i < listeFournisseurs.Count; i++)
            {
                for (int j = 0; j < 3; j += 3)
                {
                    listeFinaleCommande.Add(tab[i, 0]);
                    listeFinaleCommande.Add(tab[i, 1]);
                    listeFinaleCommande.Add(tab[i, 2]);
                }
            }
            return listeFinaleCommande;
        }

        /// <summary>
        /// fonction qui édite un fichier XML
        /// càd qu'elle sérialise la classe qu'on a créé ultérieurement qui contient la liste commande fournisseur
        /// elle crée un nouveau fichier XML situé dans bin/Debug
        /// </summary>
        static void EditeurXML()
        {
            CommandeFournisseur commande = new CommandeFournisseur { listeCommandeFournisseur = ListeCommandeFournisseur(ListeFournisseurs(), ListeProduits(), QuantiteACommander(ListeQuantiteMinTriee(), ListeQuantiteMaxTriee())) };
            XmlSerializer xs = new XmlSerializer(typeof(CommandeFournisseur));
            using (StreamWriter wr = new StreamWriter("commande.xml"))
            {
                xs.Serialize(wr, commande);
            }

        }

        /// <summary>
        /// Permet de faire la fonction réapprovisionnement
        /// comprend la fonction de mise à jour des quantités de recettes et l'éditeur de fichier XML
        /// </summary>
        static void Reapprovisionnement()
        {
            Console.WriteLine("\nVous êtes dans la rubrique Réapprovisionnement.\nVoici la liste des produits disponibles.");
            Connexion_BDD1("select nomProduit from produit;");
            Console.WriteLine("\nEcrivez le nom du produit dont vous souhaitez vérifier les stocks disponibles > ");

            string aliment = Console.ReadLine();
            int compteur = 0;

            List<string> listeProduits = ConnexionBDDUneColonne("select nomProduit from produit;");

            while (compteur == 0)
            {
                foreach (string value in listeProduits)
                {
                    if (aliment == value)
                    {
                        compteur++;
                    }
                }
                if (compteur == 0)
                {
                    Console.WriteLine("Produit indisponible\nVeuillez rentrer un autre produit > ");
                    aliment = Console.ReadLine();
                }
            }
            Console.WriteLine("\nAppuyez sur une touche pour continuer\n");
            Console.ReadKey();
            MiseAJour(aliment);

            Console.WriteLine("\nVoici maintenant la liste des commandes fournisseurs, édité au format XML.\nVous retrouverez ce fichier dans le dossier Debug du projet.\n");
            Console.WriteLine("\nAppuyez sur une touche pour continuer\n");
            Console.ReadKey();
            bool ok = false;
            EditeurXML();
            ok = true;
            if (ok)
            {
                Console.WriteLine("Le fichier a été crée avec succès !\nIl porte le nom de 'commande' sous format XML");
                Console.WriteLine("\nAppuyez sur une touche pour continuer\n");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// fonction qui détermine le CdR de la semaine et qui retourne son nom 
        /// effectue le calcul en partant de la date d'aujourd'hui et compte 7 jours en arrière, et voit quel CdR a eu le plus de recettes commandées 
        /// </summary>
        /// <returns>CdR de l semaine </returns>
        static string CdROfTheWeek()
        {
            string CdR = "";
            DateTime thisDate = DateTime.Now;
            CultureInfo culture = new CultureInfo("ja-JP");
            string dateFin = thisDate.ToString("d", culture);
            string dateFinaleFin = "";
            for (int i = 0; i < dateFin.Length; i++)
            {
                if (i == 4 || i == 7)
                {
                    dateFinaleFin += "-";
                }
                else
                {
                    dateFinaleFin += dateFin[i];
                }
            }
            DateTime debutSemaine = thisDate.AddDays(-7);
            string dateDebut = debutSemaine.ToString("d", culture);
            string dateFinaleDebut = "";
            for (int i = 0; i < dateDebut.Length; i++)
            {
                if (i == 4 || i == 7)
                {
                    dateFinaleDebut += "-";
                }
                else
                {
                    dateFinaleDebut += dateDebut[i];
                }
            }
            List<string> listeQuantiteHebdo = ConnexionBDDDeuxColonne("select C.pseudo, sum(Con.quantiteRecette) from client C, recette R, contient Con, commande Comm where " +
                "Comm.numeroCommande = Con.numeroCommande and Con.numeroRecette = R.numeroRecette and R.pseudo = C.pseudo and C.CdR = true and " +
                "Comm.dateCommande between '" + dateFinaleDebut + "' AND '" + dateFinaleFin + "' group by C.pseudo;");
            int compteurMax = 0;
            for (int i = 1; i < listeQuantiteHebdo.Count; i += 2)
            {
                if (Convert.ToInt32(listeQuantiteHebdo[i]) > compteurMax)
                {
                    compteurMax = Convert.ToInt32(listeQuantiteHebdo[i]);
                    CdR = listeQuantiteHebdo[i - 1];
                }
            }
            return CdR;
        }

        /// <summary>
        /// fonction qui affiche directement sur la console le top 5 des recettes les plus commandées sur le site
        /// ne retourne rien
        /// </summary>
        static void Top5Recettes()
        {
            List<string> listeTopRecettes = ConnexionBDDSeptColonne("select numeroRecette, nomRecette, typeRecette, descriptif, prixVente, pseudo, " +
                "compteur from recette order by compteur DESC;");
            for (int i = 0; i < 35; i += 7)
            {
                Console.WriteLine("La recette numéro " + listeTopRecettes[0 + i] + " porte le nom de " + listeTopRecettes[1 + i] + ", est de type " +
                    listeTopRecettes[2 + i] + ", " + listeTopRecettes[3 + i] + ".\nCe plat coûte " + listeTopRecettes[4 + i] + ", a été crée par " +
                    listeTopRecettes[5 + i] + " et a été commandé " + listeTopRecettes[6 + i] + " fois depuis l'ouverture du site Cooking.");
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// fonction qui affiche sur la console le CdR le plus prisé du site
        /// et affiche également les liste de ses 5 recettes les plus commandées
        /// </summary>
        static void CdRDOr()
        {
            string CdR = "";
            List<string> listeTopCdR = ConnexionBDDDeuxColonne("select C.pseudo, sum(R.compteur) from client C, recette R where CdR = true AND" +
                " C.pseudo = R.pseudo group by C.pseudo order by R.compteur DESC;");
            int compteurMax = 0;
            for (int i = 1; i < listeTopCdR.Count; i += 2)
            {
                if (Convert.ToInt32(listeTopCdR[i]) > compteurMax)
                {
                    compteurMax = Convert.ToInt32(listeTopCdR[i]);
                    CdR = listeTopCdR[i - 1];
                }
            }
            Console.WriteLine("Le CdR d'Or n'est autre que " + CdR + " et comptabilise à son actif près de " + compteurMax + " commandes. Un record !");
            int taille = 0;
            List<string> listeTop5RecettesCdR = ConnexionBDDUneColonne("select nomRecette from recette where pseudo = '" + CdR + "';");
            Console.WriteLine("\nVoici ses recettes les plus commandées : \n");
            if (listeTop5RecettesCdR.Count < 5)
            {
                taille = listeTop5RecettesCdR.Count;
            }
            else
            {
                taille = 5;
            }
            for (int i = 0; i < taille; i++)
            {
                if (listeTop5RecettesCdR[i] != null)
                {
                    Console.WriteLine(listeTop5RecettesCdR[i]);
                }

            }
        }

        /// <summary>
        /// Affiche la liste entière de toutes les recettes disponibles
        /// Utilisé quand le gestionnaire veut supprimer une recette
        /// </summary>
        static void AfficherListeRecette()
        {
            List<string> recetteDisponible = ConnexionBDDUneColonne("select nomRecette from recette;");
            Console.WriteLine("Voici la liste des recettes disponibles :");
            int compteur = 1;
            foreach (string value in recetteDisponible)
            {
                Console.WriteLine(compteur + ". " + value);
                compteur++;
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

        }

        /// <summary>
        /// Permet de savoir si la recette rentrée en parametre fait partie de la base de donnée cooking
        /// Utilisé quand le gestionnnaire veut supprimer une recette
        /// </summary>
        /// <param name="nomrecette">nom de la recette à vérifier</param>
        /// <returns>vraie si dans la BDD, faux sinon</returns>
        static bool RecetteDansBDD(string nomrecette)
        {
            bool reponse = false;
            int compteur = 0;
            List<String> listeRecettes = ConnexionBDDUneColonne("select nomRecette from recette");
            for (int i = 0; i < listeRecettes.Count; i++)
            {
                if (nomrecette == listeRecettes[i])
                {
                    compteur++;
                }
            }
            if (compteur != 0)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de supprimer une recette de la BDD et renvoie un boolèen confirmant qu'elle a été bien supprimée
        /// </summary>
        /// <param name="nomrecette">nom de la recette a supprimer</param>
        /// <returns>Vraie si la recette a été supprimé, faux sinon</returns>
        static bool SupprimerRecette(string nomrecette)
        {
            bool recetteSupprimer = false;
            Connexion_BDD2("delete from recette where nomRecette = '" + nomrecette + "';");
            recetteSupprimer = true;
            return recetteSupprimer;
        }

        /// <summary>
        /// Permet d'afficher la liste des cuisiniers
        /// Utilisé quand le gestionnaire veut supprimer un cuisinier
        /// </summary>
        static void AfficherListeCuisinier()
        {
            List<string> cdRDisponible = ConnexionBDDUneColonne("select pseudo from client where CdR = true;");
            Console.WriteLine("Voici la liste des CdR disponibles :");
            int compteur = 1;
            foreach (string value in cdRDisponible)
            {
                Console.WriteLine(compteur + ". " + value);
                compteur++;
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet de vérifier si le nom du cuisinier passé en parametre appartient bien a la BDD cooking
        /// Renvoie un bolléen conirmant que le CdR appartient bien à la BD
        /// Utilisé quand le gestionnaire veut supprimer un cuisinier
        /// </summary>
        /// <param name="cuisinierValide">le nom du cuisinier</param>
        /// <returns>Vraie si le cuisinier est dans la BDD, faux sinon</returns>
        static bool CuisinierDansBDD(string nomCuisinier)
        {
            bool reponse = false;
            int compteur = 0;
            List<String> listeCdR = ConnexionBDDUneColonne("select pseudo from client where CdR = true;");
            for (int i = 0; i < listeCdR.Count; i++)
            {
                if (nomCuisinier == listeCdR[i])
                {
                    compteur++;
                }
            }
            if (compteur != 0)
            {
                reponse = true;
            }
            return reponse;
        }

        /// <summary>
        /// Permet de supprimer un cuisinier (ainsi que toutes ses recettes de la BDD)
        /// </summary>
        /// <param name="nomcuisinier">nom du cuisinier a supprimer</param>
        /// <returns>Vraie si le cuisinier (et ses recettes) a bien été supprimé, faux sinon</returns>
        static bool SupprimerCuisinier(string nomcuisinier)
        {
            bool cuisinierSupprimer = false;
            Connexion_BDD2("delete from client where pseudo = '" + nomcuisinier + "' and CdR = true;");
            cuisinierSupprimer = true;
            return cuisinierSupprimer;
        }

        //----------------------------------------------------------------------------------



        //---------------------Utilisé dans le mode demo------------------------------------

        /// <summary>
        /// Permet de savoir si le produit passé en parametre est dans la liste passé en parametre
        /// </summary>
        /// <param name="produit">produit</param>
        /// <param name="listeProduit">liste de produit</param>
        /// <returns>Vraie si le produit est dans la liste, faux sinon</returns>
        static bool ProduitDansListe(string produit, List<string> listeProduit)
        {
            bool reponse = false;
            foreach (string value in listeProduit)
            {
                if (produit == value)
                {
                    reponse = true;
                }
            }
            return reponse;
        }


        /// <summary>
        /// Permet de faire le mode démo
        /// Utilisé quand l'utilisateur veut faire le mode démo
        /// </summary>
        static void ModeDemo()
        {
            Console.WriteLine("\nBienvenue dans le mode demo");

            //Nombre de clients
            Console.WriteLine("Vous allez d'abord voir le nombre de client qui consitue notre BDD");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            List<string> nbClient = ConnexionBDDUneColonne("select count(*) from client;");
            Console.WriteLine("\nLe nombre de client est " + nbClient[0]);
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            //Nombre des CdR
            Console.WriteLine("\nVous allez maintenant voir le nombre de Cdr parmis les clients de notre BDD");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            List<string> nbCdR = ConnexionBDDUneColonne("select count(*) from client where CdR = true;");
            Console.WriteLine("\nLe nombre de CdR est " + nbCdR[0]);
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            //Nom des cdr ainsi que le nombre total de ses recettes commandés
            Console.WriteLine("\nVous allez maintenant voir les noms des Cdr et le nombre total de leur recettes commandées");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            Console.WriteLine("");
            List<string> nomCdr = ConnexionBDDUneColonne("select pseudo from client where CdR = True;");
            Console.WriteLine("Voici la liste des cuisiniers : ");
            for (int i = 0; i < nomCdr.Count; i++)
            {
                Console.WriteLine((i + 1) + ". Le/la cuisinier : " + nomCdr[i]);
            }
            Console.WriteLine("\nEt voici la liste des cuisinier qui ont créé une ou plusieurs recettes");
            List<string> nomCdrNbRecette = ConnexionBDDDeuxColonne("select C.pseudo, sum(R.compteur) from client C, recette R where R.Pseudo = C.Pseudo group by C.pseudo;");
            for (int i = 0; i < nomCdrNbRecette.Count; i += 2)
            {
                Console.WriteLine("Le cuisinier : " + nomCdrNbRecette[i] + " a un nombre totale de recette commandé égale a : " + nomCdrNbRecette[i + 1]);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            //Nombre de recettes
            Console.WriteLine("\nVous allez maintenant voir le nombre de recettes de notre BDD");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            List<string> nbRecette = ConnexionBDDUneColonne("select count(*) from recette;");
            Console.WriteLine("\nLe nombre de recette est " + nbRecette[0]);
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            //Liste des produits ayant une quantité de stock <= 2*leur quantité minimale
            Console.WriteLine("\nVous allez maintenant voir les produits dont le stock actuel est inférieur ou égale a 2 fois le stock minimal");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.WriteLine("");
            Console.ReadKey();
            List<string> listeProduit = ConnexionBDDUneColonne("select nomProduit from produit where stockActuel <= 2*stockMinimal;");
            for (int i = 0; i < listeProduit.Count; i++)
            {
                Console.WriteLine("Le produit : " + listeProduit[i] + " a un stock actuel qui est inférieur ou égale a 2 fois son stock minimal");
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();

            // Saisie au clavier (par l'evaluateur) d'un des produits de la liste précédente puis affichage de la liste de leur recette (leur nom)
            // utilisant ce produit et de la quantité utilisé dans cette recette
            Console.WriteLine("\nVous avez maintenant la possibilité de saisir l'un des produit au dessus, afin d'en afficher la liste des recettes où il est présent");
            Console.Write("Saisir le produit > ");
            string produit = Console.ReadLine();
            bool produitCorrect = ProduitDansListe(produit, listeProduit);
            while (produitCorrect == false)
            {
                Console.WriteLine("\nRéponse incorrect");
                Console.Write("Saisir le produit > ");
                produit = Console.ReadLine();
                produitCorrect = ProduitDansListe(produit, listeProduit);
            }
            Console.WriteLine("\nVoici la liste des recettes que le produit " + produit + " compose :");
            string commande = "select R.nomRecette, U.quantiteProduits from utilise U, recette R where U.nomProduit = " + '\u0022' + produit + '\u0022' + " and U.numeroRecette = R.numeroRecette;";
            List<string> listeRecetteQuantite = ConnexionBDDDeuxColonne(commande);
            for (int i = 0; i < listeRecetteQuantite.Count; i += 2)
            {
                Console.WriteLine(listeRecetteQuantite[i] + " utilise " + listeRecetteQuantite[i + 1] + " " + produit);
            }
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
        }
        //----------------------------------------------------------------------------------


        //--------------------Utilisé pour reset la base------------------------------------

        /// <summary>
        /// Permet de lire un fichiers .txt et de stocker ses données dans une liste
        /// Utilisa dans la fonction reset base pour lire les fichier qui permettent de reset la base
        /// </summary>
        /// <param name="NomFichier">nom du fichier a lire</param>
        /// <returns>liste avec les élément du fichier</returns>
        static List<string> LireFichier(string NomFichier)
        {
            List<string> fichier = new List<string>();
            try
            {
                StreamReader sr = new StreamReader(NomFichier);
                string ligne = "";
                while (sr.EndOfStream == false)
                {
                    ligne = sr.ReadLine();
                    fichier.Add(ligne);
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return fichier;
        }

        /// <summary>
        /// Permet de reset la base de donnée
        /// tres utile lorsque nous testons nos fonctions mais que nous voulons revenir a la base de donnée depart
        /// </summary>
        static void ResetBase()
        {
            //D'abord on stock nos 3 fichiers qui nous permette de reset la base
            List<string> fichier = LireFichier("Drop_data.txt");
            List<string> fichier2 = LireFichier("Creation_table.txt");
            List<string> fichier3 = LireFichier("Peuplement_table.txt");
            //Ensuite on effectue, ligne par ligne, les commandes de chaqe fichier
            //en premier drop data
            foreach (string value in fichier)
            {
                Connexion_BDD2(value);
            }
            //en deuxieme création de table
            foreach (string value in fichier2)
            {
                Connexion_BDD2(value);
            }
            //en troisième on peuple les tables
            foreach (string value in fichier3)
            {
                Connexion_BDD2(value);
            }
        }
        //---------------------------------------------------------------------------------


        /// <summary>
        /// Permet d'executer le menu cooking
        /// </summary>
        static void MenuCooking()
        {
            int reponse1 = 0;
            while (reponse1 != 5)
            {
                //On donne d'abord les 4 possibilités si-dessous
                Console.WriteLine("Bienvenue sur le site cooking !");
                Console.WriteLine("Que voulez vous faire aujourd'hui ?");
                Console.WriteLine("1. Vous identifier ?");
                Console.WriteLine("2. Vous etes un gestionnaire cooking");
                Console.WriteLine("3. Executez le mode démo");
                Console.WriteLine("4. Reset la base de donnée");
                Console.WriteLine("5. Quittez totalement le site");
                Console.Write("Tapez le chiffre de votre choix > ");
                reponse1 = Convert.ToInt32(Console.ReadLine());
                //Tant que la reponse1 n'est pas bonne continuer de la redemander
                while (reponse1 > 5 || reponse1 < 1)
                {
                    Console.WriteLine("La réponse est incorrect");
                    Console.WriteLine("Que voulez vous faire aujourd'hui ?");
                    Console.WriteLine("1. Vous identifier ?");
                    Console.WriteLine("2. Vous etes un gestionnaire cooking");
                    Console.WriteLine("3. Executez le mode démo");
                    Console.WriteLine("4. Reset la base de donnée");
                    Console.WriteLine("5. Quittez totalement le site");
                    Console.Write("Tapez le chiffre de votre choix > ");
                    reponse1 = Convert.ToInt32(Console.ReadLine());
                    //On effectue un retour a la ligne pour un affichage plus propre
                    Console.WriteLine("");
                }
                //On effectue un retour a la ligne pour un affichage plus propre
                Console.WriteLine("");
                // Réponse1 = 1 le client veut s'identifier:
                if (reponse1 == 1)
                {
                    // On demande si il possède un compte cooking
                    Console.WriteLine("Avez vous un compte ?");
                    Console.WriteLine("1. si oui");
                    Console.WriteLine("2. si vous ne possedez pas de compte et vous voulez en créer un");
                    Console.Write("Tapez le chiffre de votre choix > ");
                    int reponse2 = Convert.ToInt32(Console.ReadLine());
                    while (reponse2 > 2 || reponse2 < 1)
                    {
                        Console.WriteLine("La réponse est incorrect");
                        Console.WriteLine("1. si oui");
                        Console.WriteLine("2. si vous ne possedez pas de compte et vous voulez en créer un");
                        Console.Write("Tapez le chiffre de votre choix > ");
                        reponse2 = Convert.ToInt32(Console.ReadLine());
                        //On effectue un retour a la ligne pour un affichage plus propre
                        Console.WriteLine("");
                    }
                    //On effectue un retour a la ligne pour un affichage plus propre
                    Console.WriteLine("");
                    string identifiant = "";
                    // Réponse2 = 1 il possède un compte: on vérifie son identifiant et son mdp
                    if (reponse2 == 1)
                    {
                        identifiant = VerifierCompte();
                        // Le compte est verifié on lui donne acces au site cooking
                        //On effectue un retour a la ligne pour un affichage plus propre
                        Console.WriteLine("");

                    }
                    // Réponse2 = 2 il ne possède pas de compte, on créer un compte
                    if (reponse2 == 2)
                    {
                        identifiant = CreerCompte();
                        // Le compte est créée et on lui donne acces au site cooking
                        //On effectue un retour a la ligne pour un affichage plus propre
                        Console.WriteLine("");
                    }
                    //Le compte est soit créer, soit l'utilisateur s'est identifié, on donne donc acces au site cooking

                    // On va maintenant demande au client si il est cdr ou non:
                    //On effectue un retour a la ligne pour un affichage plus propre
                    Console.WriteLine("");
                    Console.WriteLine("Etes vous Cdr ?");
                    Console.WriteLine("1. Oui");
                    Console.WriteLine("2. Non");
                    Console.Write("Tapez le chiffre de votre choix > ");
                    int reponse3 = Convert.ToInt32(Console.ReadLine());
                    while (reponse3 > 2 || reponse3 < 1)
                    {
                        Console.WriteLine("Etes vous Cdr ?");
                        Console.WriteLine("1. Oui");
                        Console.WriteLine("2. Non");
                        Console.Write("Tapez le chiffre de votre choix > ");
                        reponse3 = Convert.ToInt32(Console.ReadLine());
                    }
                    //On effectue un retour a la ligne pour un affichage plus propre
                    Console.WriteLine("");
                    bool boolCdr = ClientCdr(identifiant);
                    if (boolCdr == false && reponse3 == 1)
                    {
                        Console.WriteLine("Bah alors petit coquin on est pas un Cdr");
                        //On effectue un retour a la ligne pour un affichage plus propre
                        Console.WriteLine("");

                    }
                    // On va créer une boucle qui va permettre a l'ulisateur de soit rester dans le menu soit le quitter.
                    bool continuer = true;
                    while (continuer == true)
                    {
                        // BoolCdr = true, donc si c est un Cdr on lui affiche le menu cooking suivant
                        //On effectue un retour a la ligne pour un affichage plus propre
                        Console.WriteLine("");
                        if (boolCdr == true)
                        {
                            Console.WriteLine("Que voulez vous faire ?");
                            Console.WriteLine("1. Passer une commande");
                            Console.WriteLine("2. Créer une recette");
                            Console.WriteLine("3. Consulter le solde de cook");
                            Console.WriteLine("4. Afficher la liste de vos recettes");
                            Console.WriteLine("5. Quitter le menu");
                            Console.Write("Tapez le chiffre de votre choix > ");
                            int reponse5 = Convert.ToInt32(Console.ReadLine());
                            while (reponse5 > 5 || reponse5 < 1)
                            {
                                Console.WriteLine("Que voulez vous faire ?");
                                Console.WriteLine("1. Passer une commande");
                                Console.WriteLine("2. Créer une recette");
                                Console.WriteLine("3. Consulter le solde de cook");
                                Console.WriteLine("4. Afficher la liste de vos recettes");
                                Console.WriteLine("5. Quitter le menu");
                                Console.Write("Tapez le chiffre de votre choix > ");
                                reponse5 = Convert.ToInt32(Console.ReadLine());
                            }
                            //On effectue un retour a la ligne pour un affichage plus propre
                            Console.WriteLine("");
                            // Reponse5 = 1 , on passe une commande
                            if (reponse5 == 1)
                            {
                                PasserComande(identifiant);
                                //On effectue un retour a la ligne pour un affichage plus propre
                                Console.WriteLine("");
                            }
                            // Reponse5 = 2 , on creer une recette
                            if (reponse5 == 2)
                            {
                                CreerRecette(identifiant);
                                //On effectue un retour a la ligne pour un affichage plus propre
                                Console.WriteLine("");
                            }
                            if (reponse5 == 3)
                            {
                                ConsulterLeSolde(identifiant);
                                //On effectue un retour a la ligne pour un affichage plus propre
                                Console.WriteLine("");
                            }
                            if (reponse5 == 4)
                            {
                                AfficherListeRecetteCdr(identifiant);
                                //On effectue un retour a la ligne pour un affichage plus propre
                                Console.WriteLine("");
                            }
                            if (reponse5 == 5)
                            {
                                continuer = false;
                            }
                        }
                        // Si c est pas un cdr
                        if (boolCdr == false)
                        {
                            Console.WriteLine("Que voulez vous faire ?");
                            Console.WriteLine("1. Passer une commande");
                            Console.WriteLine("2. Consulter le solde de cook");
                            Console.WriteLine("3. Quitter le menu");
                            Console.Write("Tapez le chiffre de votre choix > ");
                            int reponse6 = Convert.ToInt32(Console.ReadLine());
                            while (reponse6 > 3 || reponse6 < 1)
                            {
                                Console.WriteLine("Que voulez vous faire ?");
                                Console.WriteLine("1. Passer une commande");
                                Console.WriteLine("2. Consulter le solde de cook");
                                Console.WriteLine("3. Quitter le menu");
                                Console.Write("Tapez le chiffre de votre choix > ");
                                reponse6 = Convert.ToInt32(Console.ReadLine());
                            }
                            //On effectue un retour a la ligne pour un affichage plus propre
                            Console.WriteLine("");
                            // Reponse3 = 1 , on passe une commande
                            if (reponse6 == 1)
                            {
                                PasserComande(identifiant);
                                //On effectue un retour a la ligne pour un affichage plus propre
                                Console.WriteLine("");
                            }
                            // Reponse3 = 2 , on creer une recette
                            if (reponse6 == 2)
                            {
                                ConsulterLeSolde(identifiant);
                                //On effectue un retour a la ligne pour un affichage plus propre
                                Console.WriteLine("");
                            }
                            if (reponse6 == 3)
                            {
                                continuer = false;
                            }
                        }
                    }
                }
                //Réponse1 = 2 le gestionnnaire veut avoir acces a ses données
                if (reponse1 == 2)
                {
                    Console.WriteLine("Que voulez-vous faire aujourd'hui ?");
                    Console.WriteLine("1. Voir le tableau de bord de la semaine");
                    Console.WriteLine("2. Voir le réaprovisionnement des produits");
                    Console.WriteLine("3. Supprimer une recette");
                    Console.WriteLine("4. Supprimer un cuisinier et toutes ses recettes");
                    Console.Write("Tapez le chiffre de votre choix > ");
                    int reponse7 = Convert.ToInt32(Console.ReadLine());
                    //Tant que la reponse1 n'est pas bonne
                    while (reponse7 > 4 || reponse7 < 1)
                    {
                        Console.WriteLine("Que voulez vous faire aujourd'hui ?");
                        Console.WriteLine("1. Voir le tableau de bord de la semaine");
                        Console.WriteLine("2. Voir le réaprovisionnement des produits");
                        Console.WriteLine("3. Supprimer une recette");
                        Console.WriteLine("4. Supprimer un cuisinier et toutes ses recettes");
                        Console.Write("Tapez le chiffre de votre choix >");
                        reponse7 = Convert.ToInt32(Console.ReadLine());
                    }
                    // Si reponse7 == 1 voir le tableau de la semaine
                    if (reponse7 == 1)
                    {
                        Console.WriteLine("\nTout d'abord, voici le CdR de la semaine ");
                        Console.WriteLine("Appuyez sur une touche pour continuer");
                        Console.ReadKey();

                        Console.WriteLine("\nEt le CdR qui a été le plus commandé cette semaine n'est autre que : " + CdROfTheWeek() + "\nBravo à lui !\n");
                        Console.WriteLine("Passons maintenant au top 5 des recettes les plus commandées cette semaine");
                        Console.WriteLine("Appuyez sur une touche pour continuer");
                        Console.ReadKey();

                        Console.WriteLine("\nVoici le top 5 des recettes de la semaine : \n");
                        Top5Recettes();
                        Console.WriteLine("Appuyez sur une touche pour continuer");
                        Console.ReadKey();

                        Console.WriteLine("Enfin, découvrons maintenant le CdR d'Or, celui qui bat tous les records depuis la création du site !");
                        Console.WriteLine("Appuyez sur une touche pour continuer\n");
                        Console.ReadKey();
                        CdRDOr();

                        Console.WriteLine("\nAppuyez sur une touche pour continuer");
                        Console.ReadKey();

                    }
                    // Si reponse7 == 2 voir le réapprovisionnement des produits
                    if (reponse7 == 2)
                    {
                        Reapprovisionnement();
                    }
                    // Si reponse7 == 3 supprimer une recette
                    if (reponse7 == 3)
                    {
                        Console.WriteLine("\nVoici la liste des recettes :\n");
                        AfficherListeRecette();
                        Console.WriteLine("\nQuelle est la recette que vous voulez supprimer ?");
                        Console.Write("Tapez la réponse > ");
                        string nomrecette = Console.ReadLine();
                        bool recetteValide = RecetteDansBDD(nomrecette);
                        while (recetteValide == false)
                        {
                            Console.WriteLine("\nRecette incorrect");
                            Console.WriteLine("Quelle est la recette que vous voulez supprimer ?");
                            Console.Write("Tapez la réponse > ");
                            nomrecette = Console.ReadLine();
                            recetteValide = RecetteDansBDD(nomrecette);
                        }
                        bool recetteSupprimer = SupprimerRecette(nomrecette);
                        if (recetteSupprimer == true)
                        {
                            Console.WriteLine("\nLa recette " + nomrecette + " a été supprimée avec succès !\n");
                        }

                    }
                    // Si reponse7 == 4 supprimer un cuisinier et toutes ses recettes
                    if (reponse7 == 4)
                    {
                        Console.WriteLine("\nVoici la liste des cuisiniers : \n");
                        AfficherListeCuisinier();
                        Console.WriteLine("\nQuelle est le/la cuisinier/e que vous voulez supprimer ?");
                        Console.Write("Tapez la réponse > ");
                        string nomcuisinier = Console.ReadLine();
                        bool cuisinierValide = CuisinierDansBDD(nomcuisinier);
                        while (cuisinierValide == false)
                        {
                            Console.WriteLine("\nNom du cuisinier incorrect");
                            Console.WriteLine("Quelle est le cuisinier que vous voulez supprimer ?");
                            Console.Write("Tapez la réponse > ");
                            nomcuisinier = Console.ReadLine();
                            cuisinierValide = RecetteDansBDD(nomcuisinier);
                        }
                        //On effectue un retour a la ligne pour un affichage plus propre
                        Console.WriteLine("");
                        bool cuisinierSupprimer = SupprimerCuisinier(nomcuisinier);
                        if (cuisinierSupprimer == true)
                        {
                            Console.WriteLine("\nLe cuisinier " + nomcuisinier + " a été supprimé avec succès, ainsi que toutes ses recettes !\n");
                        }
                    }
                }
                //Réponse1 = 3 effectue le mode demo
                if (reponse1 == 3)
                {
                    ModeDemo();
                    //On effectue un retour a la ligne pour un affichage plus propre
                    Console.WriteLine("");
                }
                //Réponse1 = 4 reset la data base cooking avec les tables et tuples de depart
                if (reponse1 == 4)
                {
                    ResetBase();
                    //On effectue un retour a la ligne pour un affichage plus propre
                    Console.WriteLine("La base de donnée a bien été réinitialisé !");
                    Console.WriteLine("Appuyer sur une touche pour continuer");
                    Console.ReadKey();
                    Console.WriteLine("");
                }
            }
        }


        static void Main(string[] args)
        {
            MenuCooking();
            Console.ReadKey();
        }
    }
}


