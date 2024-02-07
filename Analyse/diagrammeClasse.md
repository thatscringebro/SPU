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
    - utilisateurId : Guid
    - ecoleId : Guid
    + Affecter(Stagiaire, mds : Mds) : void
}

class "Enseignant" AS Ens {
    - id : Guid 
    - utilisateurId : Guid
    - ecoleId : Guid
    + SuivreStagiaire(Stagiaire) : void
}

class "Maitre de stage" AS Mds {
    - id : Guid
    - idMatricule : string
    - statut : enum(Incomplet_EnAttente, Accepte, Refuse)
    - civilite : enum(M, Mme)
    - typeEmployeur : enum(CISSS, CIUSSS)
    - telMaison : string
    - accreditation : string
    - renouvellement : string 
    - actif : bool
    - commentaires : string 
    - commentairesCISSS : string
    - nomEmployeur : string
    - idStagiaire : Guid
    - idEmployeur : Guid
    - chatId : Guid
    - utilisateurId : Guid
    
    + Evaluer(Stagiaire) : void
}

class "Stagiaire" AS St{
    - id : Guid 
    - idEnseignant : Guid 
    - chatId : Guid
    - utilisateurId : Guid
    - ecoleId : Guid
    - employeurId : Guid
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
class "Ecole" AS Ec {
    - id : Guid
    - Nom : string
    - NumDeTel : string
    - adresseId : Guid
}

class Adresse {
    - id : Guid 
    - noCivique : string
    - rue : string
    - ville : string
    - province : string 
    - codePostal : string
    - pays : string
}

class "Employeur" AS Emp {
    - id : Guid 
    - adresse : Adresse
    - typeEmployeur : enum(CISSS, CIUSSS) 
    'Ca c'est pas dans la bd mais ca devrait p-t ^ 

    - adresseId : Guid
    - utilisateurid : Guid
    
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
    - dateDebut : DataeTime
    - dateFin : DataeTime
    - confirmationPresence : bool
    - commentaire : string
    - idHoraire : Guid 
}

class Message {
    - id : Guid 
    - message : string
    - dateHeure : dateTime
    - utilisateurId : Guid
    - chatId : Guid
}

class Evaluation {
    - id : Guid 
    - formulaire : string
    - idStagiaire : Guid 
    - idMds : string

    - consulter : bool
    - actif : bool 
    - estStagiaire : bool 
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
    - idCoordonateur: Guid
    - idEnseignant : Guid

    + ObtenirListMessages() : void
    + ObtenirListMds() : void
}

class Notifications {
    id : Guid

}

User <|-- Co
User <|-- Ens
User <|-- Mds
User <|-- St
User <|-- Emp
Ec <|-- User

St -- Stage
Emp -- Adresse
Stage -- Emp 
Ec -- Adresse
Message "0..*" -- "1" Mds
Message "0..*" -- "1" Stagiaire
Message "0..*" -- "1" Enseignant

Mds "1" -- "0..*" Contrat
Mds "0..*" -- "1" Stagiaire
Mds "1" -- "0..*" Horaire
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
