# Cliquely - A program to explore co-occurrence 

"Cliquely" is tool that enables the exploration of the co-occurrence patterns of proteins across a wide range of 4,742 fully sequenced genomes from the three domains of life. The data underneath Cliquely covers more than 23 million proteins divided into 404,947 orthologous clusters. 

Based on an entry protein of interest, the data set is used to construct a co-occurrence graph based on phylogenetic profiles, with edge weights representing co-occurrence probabilities. Users can direct Cliquely to work with the entire underlying data set, or specifically in the context of Bacteria, Archaea, or Eukaryota. The Bron–Kerbosch algorithm is then used in order to detect maximal cliques in this graph, i.e. fully-connected subgraphs representing meaningful biological networks from different functional categories.  Cliquely's results shed light on functional relationships between proteins and protein families, in the context of different levels of taxonomic resolution and with unprecedented underlying data set.


## Getting started

###  Installation & requirements
Cliquely can be downloaded from the following website, as a ready-to-run executable.
The software is compatible to Windows, and was tested on Windows 10 and Windows 11 machines. 

Follow these steps:
1.	Download the Cliquely zip file of the current version from https://github.com/Cliquely/Cliquely 
( at  https://github.com/Cliquely/Cliquely/releases ). Unzip it on your own machine. Note that this file contains the user manual of Cliquely.
2. 	Make sure that you place the Cliquely folder where you wish to run it.  Note that csv output files will also be placed in this folder. 
3.	To start using Cliquely, simply click the executable, and specify the entry sequence and parameters. 


### Input sequence and optional parameters

The input of Cliquely includes an entry sequence of interest in Fasta
format. The entry sequence is used to search for cliques on a
co-occurrence graph, and should be copied to the first text box in the
main screen of the software.

Four additional parameters and options below the main text box that
users can use in order to refine their investigation:
1. *Probability*: probability of co-occurrence (*P<sub>co</sub>*) for pairs of
    proteins in the cliques to be found in the same organism.

2. *Max Clique Size*: maximal size for the identified cliques. This
    parameter allows users to limit run-time for runs that turn out to
    be too promiscuous - and may lead to results that are not of
    biological interest.

3. *Max Cliques*: in a similar way to *Max Clique Size*, this parameter
    can be used to control promiscuous runs.

4. *Search in*: this selection menu allows users to choose between
    running their entry protein vs. the entire underlying orthology data
    set that Cliquey uses, or focus either on *Archaea*, *Bacteria* or
    *Eukaryota*.

The parameter which controls the very definition of similarity within
the reported co-occurrence cliques is *Probability*, the specified
co-occurrence probability threshold (*P<sub>co</sub>*). Cliquey also utilizes
this threshold to make the algorithm more efficient, pruning the full
co-occurrence graph accordingly. Nodes and edges that are of no
relevance to the entry (input) protein are removed, and edges with
weights that are smaller than the chosen threshold (*P<sub>co</sub>*) are also
removed.


## Running Process

### Algorithm pipeline

A description of the algorithm pipeline, from input entry to the desired
output is provided in the following figure:

<img src="https://user-images.githubusercontent.com/21021560/149641216-b572851b-c1be-47dc-8aa8-a6ca1bad2699.png" width="60%">

### Step-by-step running illustration

Following download and unzipping, users can execute Cliquely by double
clicking the exe file. The following screen captures illustrate the
process from the user\'s angle:

1. Main screen as you enter the program. The user is asked to enter
choices - entry sequence, *Probability* (*P<sub>co</sub>*, in the range 0-1),
*Max clique size*, *Max cliques*, dropdown menu for the \"*Search in*\"
option. The program\'s defaults is *P<sub>co</sub>* =0.7, this will perform well
in in most cases, but users are encouraged to run Cliquely for a range
of *P<sub>co</sub>* values, and choose the best *P<sub>co</sub>* for their purposes.
<img src="https://user-images.githubusercontent.com/21021560/149641219-b3519907-d2a9-4357-922f-544105b3034d.png" width="60%">

2. After you have filled all of the required fields you may click on
\"Search cliques\" and the program will start to calculate the cliques
of the entered FASTA sequence, this process may take a few minute.
<img src="https://user-images.githubusercontent.com/21021560/149641223-f6a735f0-a75a-4f3f-a227-e8359e361f82.png" width="60%">

3. When the program finished calculating the cliques of the protein
it will show the results within Cliquely\'s window.
<img src="https://user-images.githubusercontent.com/21021560/149641227-e64af401-cf65-4e8d-b43e-755a9b1a9a65.png" width="60%">

4. Cliquely also exports the results to a csv file, and it may be
easier to view the results by opening this file, located in the
executable folder.
<img src="https://user-images.githubusercontent.com/21021560/149641234-bf93f302-582d-4f76-93bc-1eeb5f061477.png" width="60%">


### Output

The output is provided as a table of cliques, and can be viewed either
in the program\'s screen or using the csv output file. This table
contains the following:

Rows:\
Each row in the output table is a clique.

Columns:

* **Gene** - The query gene that was entered by the user. If the entry
sequence was not 100% matched to our cluster representatives, the Gene
may be one of BlastP matches.
* **Probability** - The *P<sub>co</sub>* value provided by the user.
* **Incidence** - The number of organisms within the \"*Search in*\"
choice of the user that support the clique.
* **Count** - The number of proteins in the clique.
* The columns that follow after \"*Count*\" detail the members of the
clique.


## Appendix A

### Example for the calculation of *P<sub>co</sub>*

Example for the relationship of the proteins across a universe of 10
Bacterial organisms. The table indicates which organisms contain which
proteins. For Example, Protein1 is part of the genome of Bacteria1, but
is not part of the genome of Bacteria10.

|            | Protein1 | Protein2 |
|:----------:|:--------:|:--------:|
| Bacteria1  |     X    |          |
| Bacteria2  |     X    |     X    |
| Bacteria3  |     X    |     X    |
| Bacteria4  |     X    |          |
| Bacteria5  |     X    |     X    |
| Bacteria6  |          |     X    |
| Bacteria7  |     X    |     X    |
| Bacteria8  |          |     X    |
| Bacteria9  |     X    |     X    |
| Bacteria10 |          |     X    |

Out of the 8 times Protein2 is present, in 5 times Protein1 is also
present; Out of the 7 times Protein1 is present, in 5 times Protein1 is
also present, thus

<img src="https://latex.codecogs.com/png.image?\dpi{110}&space;\bg_white&space;\inline&space;P\left(&space;Protein1|Protein2&space;\right)&space;=&space;\frac{P(Protein1&space;\cap&space;Protein2)}{P(Protein2)}&space;=&space;\frac{5}{8}" title="\bg_white \inline P\left( Protein1|Protein2 \right) = \frac{P(Protein1 \cap Protein2)}{P(Protein2)} = \frac{5}{8}" />

In the in the same way

<img src="https://latex.codecogs.com/png.image?\dpi{110}&space;\bg_white&space;\inline&space;$P\left(&space;Protein2|Protein1&space;\right)&space;=&space;\frac{P(Protein2&space;\cap&space;Protein1)}{P(Protein1)}&space;=&space;\frac{5}{7}$" title="\bg_white \inline $P\left( Protein2|Protein1 \right) = \frac{P(Protein2 \cap Protein1)}{P(Protein1)} = \frac{5}{7}$" />

\
P<sub>co</sub> is calculated as the probability of Protein1 to be present in an
organism given that Protein2 is present, multiplied by probability of
Protein2 to be present in an organism given that Protein1 is present:

<img src="https://latex.codecogs.com/png.image?\dpi{110}&space;\bg_white&space;\inline&space;$P_{co}(u,v)&space;=&space;\&space;P\left(&space;u|v&space;\right)*\&space;P\left(&space;v|u&space;\right)$" title="\bg_white \inline $P_{co}(u,v) = \ P\left( u|v \right)*\ P\left( v|u \right)$" />

<img src="https://latex.codecogs.com/png.image?\dpi{110}&space;\bg_white&space;\inline&space;$P_{co}(Protein1,Protein2)&space;=&space;\&space;P\left(&space;Protein1|Protein2&space;\right)&space;\times&space;P\left(&space;Protein2|Protein1&space;\right)$" title="\bg_white \inline $P_{co}(Protein1,Protein2) = \ P\left( Protein1|Protein2 \right) \times P\left( Protein2|Protein1 \right)$" />

<img src="https://latex.codecogs.com/png.image?\dpi{110}&space;\bg_white&space;\inline&space;$P_{co}(Protein1,Protein2)&space;=&space;\frac{5}{8}\&space;&space;\times&space;\frac{5}{7}&space;=&space;0.45$" title="\bg_white \inline $P_{co}(Protein1,Protein2) = \frac{5}{8}\  \times \frac{5}{7} = 0.45$" />

Notably, in our analysis of orthology Protein1 and Protein2 are in fact
representatives of a cluster of orthologs. Thus, the co-occurrence that
is estimated using by Cliquely using *P<sub>co</sub>* is the co-occurrence of
representatives of different orthology groups.


