# Analyse détaillée


```plantuml

hide circle


class "Utilisateur" AS User {
    - nom : string
    - prenom : string
    - courriel : string 
    - telephone : string

    + SeConnecter(username, motDePasse) : void
}

class "Coordinateur" AS Co {
    - id : Guid 

    + Affecter(Stagiaire, mds : Mds) : void
}

class "Enseignant" AS Ens {
    - id : Guid 

    + SuivreStagiaire(Stagiaire) : void
}

class "Maitre de stage" AS Mds {
    - idMatricule : string
    - statut : enum(Incomplet_EnAttente, Accepte, Refuse)
    - civilite : enum(M, Mme)
    - telMaison : string
    - accreditation : string
    - renouvellement : string 
    - actif : bool
    - commentaires : string 
    - commentairesCISSS : string
    - nomEmployeur : string
    - idStagiaire : int
    - idEmployeur : int
    
    + Evaluer(Stagiaire) : void
}

class "Stagiaire" AS St{
    - id : Guid 
    - idEnseignant : Guid 
}

class Stage {
    - idStage : Guid 
    - milieuStage : string
    - titre : string
    - signataireContrat : string
    - fonction : string 
    - signataire : string
    - tel : string
    - adresse : Adresse
    - superviseur : Mds
    - stagiaire : Stagiaire
    - secteur : string
    - program : string
    - typeStage : string
    - dateDebutStage : dateTime
    - dateFinStage : dateTime
    - superviseurCollège : Enseignant
    - poste : string

    - idEntreprise : Guid 
    - description : string
}

class Adresse {
    - id : Guid 
    - numRue : string
    - nomRue : string
    - ville : string
    - province : string 
    - codePostal : string
    - pays : string
}

class "Employeur" AS Emp {
    - id : Guid 
    - adresse : Adresse
    - typeEmployeur : enum(CISSS, CIUSSS)
    + ConsulterLstSesStagiaires() : LstStagiaires
    + ConsulterHoraireSesMds(Mds) : LstHoraireMds
}

class Horaire {
    - id : Guid 
    - idMds : Guid
    - idStagiaire : Guid 

    + ObtenirPlagesHoraires() : lstPlageHoraire
}

class PlageHoraire {
    - id : Guid 
    - dateDebut : int
    - dateFin : int
    - confirmationPresence : bool
    - idHoraire : Guid 
}

class Message {
    - id : Guid 
    - contenu : string
    - dateHeure : dateTime
    - idUser : string
}

class Evaluation {
    - id : Guid 
    - formulaire : string
    - idStagiaire : Guid 
    - idMds : string

    - isAutoEvaluation : bool
}

class Contrat {
    - id : Guid 
    - formulaire : string
    - idStagiaire : Guid 
    - idMds : Guid
}

class EmployeurMds {
    - idEntreprise : Guid 
    - idMds : Guid
}

class Chat {
    - id : Guid
    - idStagiaire : Guid
    - idEnseignant : Guid

    + ObtenirListMessages() : void
    + ObtenirListMds() : void
}

User <|-- Co
User <|-- Ens
User <|-- Mds
User <|-- St
'User <|-- Emp

St -- Stage
Emp -- Adresse
Stage -- Emp 

Message "0..*" -- "1" Mds
Message "0..*" -- "1" Stagiaire
Message "0..*" -- "1" Enseignant

Mds "1" -- "1..*" Contrat
Mds "1..*" -- "1" Stagiaire
Mds "1" -- "1..*" Horaire
Mds "1..*" -- "1" EmployeurMds

Evaluation "1..*" -- "1" Mds 
Evaluation "1" -- "1" Stagiaire

St -- Horaire

Horaire "1" -- "0..*" PlageHoraire
Horaire "1" -- "1" Stagiaire

EmployeurMds "1" -- "0..*"Emp

Enseignant "1" -- "0..*" Stagiaire

Contrat "1" -- "1" Stagiaire

Co "1" -- "1..*" Stagiaire

Chat "1" -- "1..*" Message

Chat "1" -- "1..*" Stagiaire
Chat "1" -- "1..*" Mds
Chat "1" -- "1..*" Coordinateur


'agrégation (o--) = disposable, pas dépendant
'composition (*--) = obligé d'avoir un


```
