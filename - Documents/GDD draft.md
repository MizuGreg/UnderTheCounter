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
- **avoid getting caught**: for example by not making people drink too much (because drunkards attract attention), by setting up an inconspicuous façade,

> or by spotting potential undercover policemen.

Choice is important in this game: the player's strategical decision will play a fundamental role in determining whether they will have what it takes to survive the day.

Lastly, the focus of the development of the game is set on making the game somewhat challenging, reactive, decision-based, with an innovative and engaging gameplay each day, and of course fun.

---
---

## Characters

> The main character is the bartender, which we initially know very little of. TBD

> Old bar owner, also member of the bartenders union TBD

> Another recurring character is the inspector. TBD

> 10 customers max for the prototype (for the beta maybe 20?)

> Super customers

> Mafia goon

## Story

> TBD ex proprietario bar che ti fa da tutorial, mafia (ma non troppa!), ispettore, undercover cop, ...

## World

The game world does not adhere perfectly to a historical period, but it's inspired by the prohibition era of the 1920s and 30s in the United States of America. The bar is located in a city called Korgis, situated in an unspecified region of the US.
As the game develops, various events unfold in this fictional world, first of all being a prohibition law which forbids shops from selling and distributing alcohol in any form. 

> TBD quanta mafia c'è in Korgis? chi ci vive? quanta ricchezza ci gira?

---
---

## Gameplay

The game is structured in days. Days are predominantly "self-contained", in the sense that most of the decisions taken during a day don't influence the next days. Some decisions may have an outcome on the next day.

Each day except the first one starts with setting up the shop window, deciding what kind of advertising the bar offers. The player may decide to be more or less overt in their distribution of alcoholic drinks. Then, the owner opens the bar to their patrons and the night begins.

Scripted events will happen during the shift, which will require the player to make a decision.

Based on the amount of alcoholic drinks sold, the authorities may increasingly notice that the bar is selling alcohol (by seeing drunk people come out of it), a blitz will become more and more likely this way. Arrest can be avoided if the bartender avoids getting people too drunk on their drinks (see bar mechanic below),

> or alternatively, if they successfully evade incriminating questions or hide evidence during the blitz/inspection.

Lastly, when the shift ends, the player can take a glance at all the expenses and earnings of the day and gain awareness on their economical situation.

### Shop window

> **Elements**: posters and texts, occasionally a letter (eventually, maybe: pavement sign, ...)

**Gameplay**: the player/bar owner will be able to tear down or hang posters with different purposes:
- advertising the sale of alcohol directly (e.g. a Martini poster) or covertly (e.g. "now selling Daisies and Turnscrews", referring to the Margarita and Screwdriver cocktails)
- using secret codes between bartenders in Korgis to let customers know that this bar sells alcohol
- possibly more.

These changes can affect
- the amount of customers who enter the bar, 
- how easy to achieve a police blitz is,
- the amount of tips left,

> - the difficulty of the blitz minigame
> - the duration of the day

and possibly more.

Posters may be bought, or some may be unlocked after a certain day.

### Bar view

**Elements**: area under the counter, counter (decorated with various items),

> tables and chairs, door and windows (review feasibility...)

**Gameplay**: the customers will show up at the counter, say a couple lines and then order a drink. They will pay for the cocktail after receiving it and possibly add a variable tip.

> Interesting individuals may also enter the bar apart from customers: someone with special tastes in drinks, the police inspector, mafia goons, and more.

### Cocktail preparation

> **Elements**: clickable objects like alcoholic drinks (Verlan, Caledon Ridge, Ferrucci), non-alcoholic beverages (Gryte, shaddock juice, tap water) put alongside non-interactable objects like glasses, garnishes, recipe book

**Gameplay**: in the area under the counter, the player will be able to create a cocktail out of the available ingredients, to satisfy each customer's request. The player may consult the recipe book to know which ingredient(s) to use.

- If the drink is what the customer asked for

>(and is served in a timely manner? but this bonus needs to be totally extra, not something needed to reach the end of the day. also this bonus is pretty useless if you don't have the means to spend this extra money, like posters!)

the bartender will be also tipped generously.

- If the bartender makes the wrong cocktail, the customer may get angry because of the incorrect order, not tip, and leave (but they will still get drunk if the drink is alcoholic).

- Finally, serving a correct but watered down cocktail (by adding tap water) serves as a way to keep the customer from getting drunk: they will tip less and leave, but without getting too upset.

FARLO A TABELLA (?)

### Police blitz

> **Elements**: inspector, undercover cop, ...

**Gameplay**: a police blitz may happen at some point starting from the 3rd day: this can be caused by:
- the bartender selling too much alcoholic drinks to customers
- an incriminating poster outside
> - possibly more? (tipo undercover cop)

> In this case TBD.

> An inspection will make the current customer leave without tipping (però dipende da che tipo di minigioco è!)

and scare off future ones for a few seconds.

## Other gameplay elements

### Dialogues

> The bartender may occasionally engage in conversations with customers, which may either reveal important information (e.g. future prohibition laws being discussed, secret codes for alcohol-serving bars, how to spot undercover cops) or just serve as a distraction during the game (e.g. conversations fleshing out the bartender's life and family, or the outside world). Short dialogues will be fundamental in quickly fleshing out the customer's personality and habits.
> Dialog lines will require a click to proceed to the next one; double click to display all the text at once

### Decisions

> During dialogues or indirectly by playing, the player may have to make decisions which impact what happens during the rest of the day. A correct decision in a dialogue may make a customer a regular, whereas a wrong decision while setting up the window shop may decrease the flow of customers for the day.

### Gameplay modes and other features

> An "endless mode" is being considered, where there is no story-related event and no limit on the number of days. The days progressively get harder as the player needs to avoid bankrupt and arrest.

---
---

## Media list

Sprites:
- old former bartender
- police inspector
- 10 customers
- boh

Sound:
- sounds effects for
    - recipe book
    - drinks (shaker, water/liquids pouring)
    - posters
    - dialogs?
    - clicking on buttons?

OST:
- Menu (medium length)
- Shop window (short/medium length)
- Bar (high length)
- End of day (short length)
- Win/lose tracks (short? possibly)

## Technical specifications

...

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
- note: meeting lasted from 9:30 till 20:30

### Meeting - 24/10
- defined cocktails and cocktail ingredients
- draft of cocktail creation area
- defined and sketched the 10 customers + 2 special characters present in the prototype, and fleshed out their personality and dialogues
- specified amount of customers and duration in minutes in each day

### Meeting - 25/10
- reviewed GDD draft
- brainstorming blitz minigame
- finalized bar layout and developed cocktail creation layout 
- set up Trello board and started organizing coding tasks

### Deadline (GDD and GitHub repo) - 04/11

### Deadline (prototype submission) - 08/12

### Deadline (all prototype feedbacks received) - 11/12

### Deadline (beta submission) - 12/01

### Deadline (final project submission) - 23/02
