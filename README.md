# Jumper oefening
## Tjen Heylen, Miro Graré 😀
## Oefening
- Set-up: Blokjes bewegen naar de agent toe, ze hebben een willekeurige snelheid. Afhankelijk van het soort blokje kan de agent een reward of punishment krijgen als hij ze aanraakt. De agent heeft de mogelijk om erover te springen.
- Goal: Agent springt enkel over de rode blokjes, en springt niet over een groen blokje.
- Agents: Er is een agent aanwezig.
- Agent Reward Function (independent):
    - +1 voor een groen blokje aan te raken
    - +1 voor over een rood blokje te springen
    - -1 voor over een groen blokje te springen
    - -1 voor een rood blokje aan te raken
- Behavior Parameters:
    - Vector Observation space: 8:
	    - Positie van het blokje
	    - Positie van zichzelf
	    - Soort blokje (boolean)
	    - Snelheid van het blokje (float)
    - Actions: 1 discrete action Branch, met 2 actions:
	    - input van 0 doet niets
	    - input van 1 springt
- Benchmark Mean Reward: 1

## Trainingsprocess
### Poging 1
Gelijke reward/punishment voor rood/groen blokje (1), episode eindige na 200 blokjes of wanneer de agent een fout maakte. Dit had als resultaat dat de agent stil bleef staan waardoor de reward altijd rond de 0 zat.
### Poging 2
Reward voor een groen blokje aangepast naar 0.01 en punishment bleef -1 voor een rood blokje. Hierdoor werd het belangrijker voor de agent om effectief iets te doen, maar de agent sprong nu willekeurig en de reward bleef niet goed.
### Poging 3
Hierbij hebben we de rewards terug gelijkgezet aan de punishemnt zoals in poging 1, maar eindigden we de episode na elk blokje, hierna ging het beter maar bleef hij vaak gewoon willekeurig springen.
### Poging 4
Hier begonnen we te experimenteren met twee hyperparameters: batch_size en buffer_size. We merkten dat ze nogal laag stonden dus we hebben ze hard verhoogd, wat een goed resultaat opleverde. Zoals hieronder te zien hebben we een aantal verschillende waardes geprobeerd, maar we zijn uiteindelijk beland op 256 batch_size en 2048 buffer_size.
![[./Images/jumper_graph.png]]
