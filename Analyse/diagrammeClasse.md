# Analyse détaillée


```plantuml

hide circle


class "Utilisateur" AS User {
    - id : Guid 
    - nom : string
    - prenom : string
    - courriel : string 
    - telephone : string
    - SeConnecter() : void

}

class "Coordinateur" AS Co {
    - id : Guid 
    + Affecter(stagiare : Stagiaire, mds : Mds) : void
}

class "Enseignant" AS Ens {
    - id : Guid 
    + SuivreStagiaire(stagiaire : Stagiaire) : void
}

class "Maitre de stage" AS Mds {
    - idMatricule : string
    - statut : Statut
    - civilite : Civilite
    - telMaison : string
    - accreditation : string
    - renouvellement : string 
    - actif : bool
    - commentaires : string 
    - commentairesCISSS : string
    - nomEmployeur : string
    - idStagiaire : int
    - idEmployeur : int
    
    + Evaluer(stagiaire : Stagiaire) : void
}

enum TypeEmployeur {
    CISSS
    CIUSSS
}

enum Civilite {
    M
    Mme
}

enum Statut {
    Incomplet_EnAttente
    Accepte
    Refuse
}

class "Stagiaire" AS St{
    - id : Guid 
    - idEnseignant : Guid 
    - idMds : string

    '+ ConsulterStage()
    '+ MettreAJourInfos()
    '+ Rechercher()
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
    - Stagiaire : Stagiaire
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

    + ConsulterLstSesStagiaires() : LstStagiaires
    + ConsulterHoraireSesMds(mds : Mds) : LstHoraireMds
}

class Horaire {
    - id : Guid 
    - idMds : String
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
    '- dateHeure : string? dateTime
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
    - idMds : string
}

class EmployeurMds {
    - idEntreprise : Guid 
    - idMds : string
}

class Chat {
    - id : Guid
    + ObtenirListMessage() : void
}

User <|-- Co
User <|-- Ens
User <|-- Mds
User <|-- St
'User <|-- Emp

St *-- Stage
Emp *-- Adresse
Stage o-- Emp 

Message "0..*" -- "1" Mds
Evaluation "1" -- "1" Mds 
Mds "1" -- "1..*" Contrat

Mds "1..*" -- "1" Stagiaire
Mds "1" *-- "1..*" Horaire
St *-- Horaire

Horaire "1" -- "0..*" PlageHoraire

Mds "1..*" -- "1" EmployeurMds
EmployeurMds "1" -- "0..*"Emp

Enseignant "1" -- "0..*" Stagiaire

Message "0..*" -- "1" Stagiaire

Message "0..*" -- "1" Enseignant

Evaluation "1" -- "1" Stagiaire

Contrat "1" -- "1" Stagiaire

Horaire "1" -- "1" Stagiaire

Co "1" -- "0..*" Stagiaire

'agrégation (o--) = disposable, pas dépendant
'composition (*--) = obligé d'avoir un


```
