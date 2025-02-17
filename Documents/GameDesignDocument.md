# Under the Counter

Under the Counter is a simulation game in which you impersonate a bartender trying to stay afloat and undercover as the prohibition era begins.

## Changelog

- 1.0: created GDD draft
- 1.1: updated GDD draft, highlighted uncertain sections

## Overview and vision statement

In Under the Counter you play as the owner of a bustling bar in a 20's retro-inspired city called Korgis. You will have to manage the sale of alcoholic beverages as prohibition law sets in, trying to maximize your revenue while keeping the illegal operation hidden from the authorities.

The overall mood is dynamic, challenging, pressuring and clever, while the setting is 20's retro, fancy, jazzy and old-fashioned.

The game is divided into several days, which are played consecutively until the last day is completed, or until the player loses. The objective of each day is to avoid going into the red by the end of the shift, and avoid getting caught serving alcohol.

To reach the end of the day safely, the player needs to
- **earn enough**: for example through tips, by serving the right drinks to customers, in order to be able to afford keeping the bar open another day
- **avoid getting caught**: for example by not making people drink too much (because drunkards attract attention), by setting up an inconspicuous façade, and more.

Choice is important in this game: the player's strategical decision will play a fundamental role in determining whether they will have what it takes to survive the day.

Lastly, the focus of the development of the game is set on making the game somewhat challenging, reactive, decision-based, with an innovative and engaging gameplay each day, and of course fun.

---
---

## Characters

The main character is the bartender, which we know very little of since they never speak.

The former bar owner, an old man, teaches the player the basics of bartending. He is also a member of the bartenders union.

Another recurring character is the inspector. They will warn the player of the instatiation of the prohibition law.

There will be a few customers for the prototype, but a couple more customers are planned to be added for the next releases.

Super customers, that is, customers with special interests or interactions, are planned to be added in the future.

Characters belonging to the local criminal organizations may be added in the future.

## Story

The main outline of the story is that the protagonist buys a new bar, but shortly after settling in, prohibition strikes, making the sale of alcohol illegal. The bartender will thus have to start operating in a more careful way while continuing to serve alcoholic beverages. Some factions in the city of Korgis may help or hinder them: among these are the bartenders union and the local mafia.

## World

The game world does not adhere perfectly to a historical period, but it's inspired by the prohibition era of the 1920s and 30s in the United States of America. The bar is located in a city called Korgis, situated in an unspecified region of the US.
As the game develops, various events unfold in this fictional world, first of all being a prohibition law which forbids shops from selling and distributing alcohol in any form. 

The city of Korgis is a fair one, where the police protects its citizens while fighting organized crime. Prohibition breaks this balance, making police operations harsher and more frequent, while at the same time often leaving bars and pubs at the mercy of mafia clans, which try to extort money or acquire control.

Korgis is fairly multicultural, and hosts several artistic and musical events on a daily basis. The protagonist's bar will thus be visited by customers of varying social background and economic status.

---
---

## Gameplay

The game is structured in days. Days are predominantly "self-contained", in the sense that most of the decisions taken during a day don't influence the next days. Some decisions may have an outcome on the next day.

Each day except the first one starts with setting up the shop window, deciding what kind of advertising the bar offers. The player may decide to be more or less overt in their distribution of alcoholic drinks. Then, the owner opens the bar to their patrons and the night begins.

Scripted events will happen during the shift, which will require the player to make a decision.

Based on the amount of alcoholic drinks sold, the authorities may increasingly notice that the bar is selling alcohol (by seeing drunk people come out of it), a blitz will become more and more likely this way. Arrest can be avoided if the bartender avoids getting people too drunk on their drinks (see bar mechanic below), or alternatively, if they hide evidence and successfully evade incriminating questions during the blitz.

Lastly, when the shift ends, the player can take a glance at all the expenses and earnings of the day and gain awareness on their economical situation.

### Shop window

**Elements**: posters and texts, occasionally a letter (possibly in the future: pavement sign, ...)

**Gameplay**: the player/bar owner will be able to tear down or hang posters with different purposes:
- advertising the sale of alcohol directly (e.g. a Martini poster) or covertly (e.g. "now selling Daisies and Turnscrews", referring to the Margarita and Screwdriver cocktails)
- using secret codes between bartenders in Korgis to let customers know that this bar sells alcohol
- possibly more.

These changes can affect
- how easy to achieve a police blitz is (e.g. a poster advertising alcohol will make the police far more alert),
- the amount of tips left (e.g. "Tip your local barkeeper <3"),
- the difficulty and/or duration of the blitz minigame (e.g. "We respect the police in this establishment!")
- the duration of the day (e.g. "Bar open until late today!")

and possibly more.

Posters may be bought, or some may be unlocked after a certain day.

### Bar view

**Elements**: area under the counter, counter (decorated with various items), (according to time and feasibility: tables and chairs, door and windows)

**Gameplay**: the customers will show up at the counter, say a couple lines and then order a drink. They will pay for the cocktail after receiving it and possibly add a variable tip.

In the future, interesting individuals may also enter the bar apart from customers: someone with special tastes in drinks, the police inspector, mafia goons, and more.

### Cocktail preparation

**Elements**: clickable objects like alcoholic drinks (Verlan, Caledon Ridge, Ferrucci), non-alcoholic beverages (Gryte, shaddock juice, tap water) put alongside non-interactable objects like glasses, garnishes, recipe book

**Gameplay**: in the cocktail creation area, the player will be able to create a cocktail out of the available ingredients, to satisfy each customer's request. The player may consult the recipe book to know which ingredient(s) to use.

- If the drink is what the customer asked for the bartender will be also tipped. Preparing the cocktail in a timely manner will increase the tip.

- If the bartender makes the wrong cocktail, the customer may get angry because of the incorrect order, not tip, and leave (but they will still get drunk if the drink is alcoholic).

- Finally, serving a correct but watered down cocktail (by adding tap water) serves as a way to keep the customer from getting drunk: they will tip less and leave, but without getting too upset.

Summary:

|                          | CUSTOMER OUTCOME              | TIP                                                               | DRUNKENNESS                          |   |
|--------------------------|-------------------------------|-------------------------------------------------------------------|--------------------------------------|---|
| CORRECT DRINK            | customer **satisfied**        | **yes**; **additional** tip if cocktail is served **fast enough** | **yes**                              |   |
| INCORRECT                | customer **angry/upset**      | **no**                                                            | **yes** if cocktail is **alcoholic** |   |
| CORRECT BUT WATERED DOWN | customer **mildly satisfied** | **little**                                                        | **no**                               |   |

### Police blitz

**Elements**: inspector, bottle-hiding minigame, dialogue with choices ...

**Gameplay**: a police blitz may happen at some point starting from the 4th day: this can be caused by:
- the bartender selling too many alcoholic drinks to customers (each day has a scripted maximum amount of drunk people which triggers the blitz)
- an incriminating poster outside
- possibly more

If the right conditions are satisfied the blitz will take place immediately.

In this case a custom interface will appear, letting the player know they need to hide evidence of selling alcohol before the police enters the bar.

1) First, the player will need to hide the alcoholic ingredients inside a safe within a specified time (a time bar will appear on screen for this purpose)

2) Then, upon entering, the inspector will ask a couple trick questions to the bartender.

If the player hasn't hidden all evidence in time, or responded wrongly to a question, the bar will be closed and the game ends. Otherwise the player has survived the blitz and can continue as usual. This will reset the blitz conditions.

In either case, blitz are planned to be time-consuming, so as to make the bartender lose time (and subsequently money).

An inspection will make the current customer leave and scare off future ones for a few seconds.

## Other gameplay elements

### Dialogues

The bartender will engage in very small conversations with customers, which may either reveal important information (e.g. future prohibition laws being discussed, secret codes for alcohol-serving bars, how to spot undercover cops) or just serve as a small distraction during the game. Such short dialogues can be fundamental in quickly fleshing out the customer's personality and habits.

Dialog lines will require a click to proceed to the next one; double click to display all the text at once.

### Decisions

Decisions are pragmatic in this game, in the sense that the player can make strategic decisions based on their actions. During the blitz minigame, the bartender can choose among a list of possible answers to the inspector's questions.

### Gameplay modes and other features

No additional game modes are planned at the moment.

---
---

## Media list

Sprites:
- old former bartender
- police inspector
- customers (hopefully 10 in the prototype)
- cocktail ingredients, cocktails, glasses, shaker
- counter
- possibly more in the future

Images:
- backgrounds for bar, shop window, menu

Sound:
- sounds effects for
    - recipe book
    - drinks (shaker, water/liquids pouring)
    - posters
    - possibly UI-related sounds

OST:
- Menu (medium length)
- Shop window (short/medium length)
- Bar (high length)
- End of day (short length)
- Win/lose tracks (short? possibly)

## Technical specifications

None yet.

## Team
1. Gloria Gaggelli (project leader, game designer, artist) 
2. Gregorio Dimaglie (developer)
3. Mohammad Arshya Jabbarizadegan (developer)
4. Luca Simei  (developer)
5. Riccardo Speroni (developer)

## Deadlines and team meetings

### Meeting - 13/10

- heavy brainstorming
- extensive moodboard
- initial outline of the game
- general aesthetic and feel
- poster design, character design, interior design, design of the cocktails
- discussed suitable gameplay and UI elements from similar games
- discussed what constitutes the MVP and what are instead optional additional elements, concepts and mechanics

### Meeting - 15/10

- discussed window shop mechanic, cocktail mechanic, blitz mechanic
- discussed wrong cocktails mechanic
- additional character design on the moodboard

### Meeting - 20/10

- updated character design on the moodboard
- updated GDD; highlighted uncertain sections
- discussed blitz mechanic
- defined in-game events and outlined game story
- outlined recurring and important characters
- discussed how characters and events intertwine with and influence the gameplay
- note: meeting lasted with varying participants from 9:30 till 20:30

### Meeting - 24/10
- defined cocktails and cocktail ingredients
- draft of cocktail creation area
- defined and sketched the 10 customers + 2 special characters present in the prototype, and fleshed out their personality and dialogues
- specified amount of customers and duration in minutes in each day

### Meeting - 25/10
- reviewed GDD draft
- brainstorming of blitz minigame
- finalized bar layout and developed cocktail creation layout 
- set up Trello board and started organizing coding tasks

### Meeting - 27/10
- finalized inspection mechanic and the two blitz minigames
- finalized some details on inspection and cocktail preparation layout
- completed GDD (but final revision needed)
- completed character designs
- completed setup of Trello board and GitHub repo to all 5 team members

### Meeting - 29/10
- final revision of GDD, ready for [04/11 deadline](#deadline-gdd-and-github-repo---0411)
- creation and assignment of coding tasks to developers
- creation of Gantt chart for development until beta deadline

### Deadline (GDD and GitHub repo) - 04/11

### Deadline (prototype submission) - 08/12

### Deadline (all prototype feedbacks received) - 11/12

### Deadline (beta submission) - 12/01

### Deadline (final project submission) - 23/02
