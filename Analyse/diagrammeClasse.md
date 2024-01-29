# Analyse détaillée


```plantuml

hide circle


class "Utilisateur" AS User {
    - id : int 
    - nom : string
    - prenom : string
    - courriel : string 
    - telephone : int
    - SeConnecter()

}

class "Coordonateur" AS Co {
    + Affecter()
}

class "Enseignant" AS Ens {
    - matiere : string
    + Suivre(stagiaire)
}

class "Maitre de stage" AS Mds {
    + Evaluer(stagiaire)
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
    Accepté
    Refusé
}

class "Stagiaire" AS St{
    - idEnseignant : int
    - idMds : int

    + Ajouter()
    + Consulter()
    + MettreAJour()
    + Rechercher()
}

class Stage {
    - idStage : int
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


    - idEntreprise : int
    - description : string
}

class Adresse {
    - numRue : string
    - nomRue : int
    - ville : string
    - province : string 
    - codePostal : string
    - pays : string
}

class "Employeur" AS Emp {
    - id : int
    - adresse : Adresse

    + ConsulterLstSesStagiaires()
    + ConsulterHoraireSesMds
}

class Horaire {
    - id : int
    - idMds : int
    - idStagiaire : int

    - date : date
    - heureDebut : time
    - heureFin : time
}

class Absence {
    - id : int

    - date : date
    - heureDebut : time
    - heureFin : time

    + AfficherAbsences()
}


User <|-- Co
User <|-- Ens
User <|-- Mds
User <|-- St
User <|-- Emp

St *-- Stage

Adresse *-- Emp 
Stage o-- Emp 

Mds *-- Horaire
St *-- Horaire
Absence o-- Horaire

'agrégation (o--) = disposable, pas dépendant
'composition (*--) = obligé d'avoir un


```
