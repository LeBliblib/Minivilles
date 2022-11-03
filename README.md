# Minivilles
 
README - Groupe D
(Ydris BENMANSOUR, Hélias GAMONET, Erwan TEMPLE, Yuna BONNIFET)
_________________________________________________________________
 
Règles : 
Nous avons choisi d’implémenter les règles officielles du jeu de société Minivilles. (Il faut donc avoir tous les monuments pour gagner.) 

Affichage :
- La pile / le magazin est à droite. (Ce panel est rétractable pour ne pas trop surcharger l'écran) Cliquez sur une carte pour acheter.
- Le bouton pour passer / finir son tour est le panneau avec la flèche
 
Améliorations : 
- 2 difficultés pour l’IA au choix
- Utilisation d’une interface graphique (Unity)
- Ajout de sons et effets visuels
- Feux d’artifices codé
- Animation des dés en 2D (l'effet de perspective est un twean sur le scale)

# Gestion dans le code de l’effet des cartes :
Toutes les valeurs des cartes (nom, couleur, prix, etc...) sont stockées dans des ScriptableObjects.
Une classe "Card" va permettre de faire fonctionner les cartes. Lorsque l'on veut donner une carte à un joueur on créée une nouvelle instance de cette classe en lui
donnant une référence au ScriptableObject voulu.
La carte va alors s'initialiser avec les paramètres du ScriptableObject.

Les effets des cartes sont gérés indépendemment de la classe "Card". Une classe abstraite "CardEvent", contient la structure des effets des cartes.
Ensuite, nous avons 3 classes "CoinsFromOtherEvent", "CoinsFromBankEvent" et "TradeEvent" qui héritent de "CardEvent". Ces classes permettent de gérer respectivement 
les effets de gains de pièces depuis un ou plusieurs autres joueurs (vol), de gains de pièces depuis la banque et d'échanges.
Ainsi, nous avons juste à configurer les ScriptableObjects des cartes comme nous le voulons et les "Card" que nous allons créer vont s'initialiser avec la bonne logique
(en utilisant que 5 classes, au lieu d'une classe par carte).

Les effets de gains de pièces sont assez simples (ajouter ou retirer des pièces). Mais l'effet d'échange quant à lui est plus compliqué : 
pour les joueurs humains, il repose sur un système de popups qui permettent de sélectionner les différents paramètres de l'échange.

Voici l'UML de notre projet :
![UML-Miniville](/UML Miniville.png)
