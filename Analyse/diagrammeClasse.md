# Diagramme de classe

```plantuml

hide circle


class "Utilisateur" AS User {
    - id : Guid
    - nom : string
    - prenom : string
    - mail : string 
    - telephone : string
    - nomUtilisateur : string
    - motDePasse : string
    - role : enum(Enseignant, Employeur, Coordonanateur, Stagiaire, Mds)
    + SeConnecter(username, motDePasse) : void
    + SInscrire() : void
}

class "Coordonnateur" AS Coo {
    - id : Guid 
    - utilisateurId : Guid
    - ecoleId : Guid
    + Affecter(Stagiaire, mds : Mds) : void
    + Affecter(Stagiaire, mds : Mds, ens : Enseignant) : void
}

class "Enseignant" AS Ens {
    - id : Guid 
    - utilisateurId : Guid
    - ecoleId : Guid
    + SuivreStagiaire( st : Stagiaire) : void
    + EvaluerStagiaire( st : Stagiaire) : void
}

class "Maitre de stage" AS Mds {
    - id : Guid
    - idMatricule : string
    - statut : enum(Incomplet_EnAttente, Accepte, Refuse)
    - civilite : enum(M, Mme)
    - typeEmployeur : enum(CISSS, CIUSSS)
    - telMaison : string
    - accreditation : string
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

class "Stagiaire" AS Sta{
    - id : Guid 
    - idEnseignant : Guid 
    - chatId : Guid
    - utilisateurId : Guid
    - ecoleId : Guid
    - employeurId : Guid
}

class "Ecole" AS Eco {
    - id : Guid
    - nom : string
    - numDeTel : string
    - adresseId : Guid
}

class "Adresse" AS Adr {
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
    - idUitlisateur : Guid
    - idAdresse : Guid

    + VoirHoraireMds(mds : Mds) : void
}

class "Horaire" AS Hor {
    - id : Guid 
    - idMds : Guid
    - idStagiaire : Guid 
    - dateDebutStage : DataeTime
    - dateFinStage : DataeTime
    + ObtenirPlagesHoraires() : lstPlageHoraire
}

class "PlageHoraire" AS Plh  {
    - id : Guid 
    - dateDebut : DataeTime
    - dateFin : DataeTime
    - confirmationPresence : bool
    - commentaire : string
    - idHoraire : Guid 
}

class "Message" AS Mes {
    - id : Guid 
    - message : string
    - dateHeure : dateTime
    - utilisateurId : Guid
    - idChat : Guid
}

class "Evaluation" AS Eva {
    - id : Guid 
    - formulaire : string
    - idStagiaire : Guid 
    - idMds : string
    - idEnseignant : string
    - consulter : bool
    - actif : bool 
    - estStagiaire : bool 
}


class Chat {
    - id : Guid
    - idCoordonateur: Guid
    - idEnseignant : Guid
    + ObtenirListMessages() : void
    + ObtenirListMds() : void
}


user "1"--"*" Adr
user "1"--"*" Eco
Eco "1"--"*" Adr
Coo "1"--"*" User
Coo "1"--"*" Eco
Ens "1"--"*" User
Ens "1"--"*" Eco
Emp "1"--"*" Adr
Emp "*"--"1" User
Chat "1"--"*" Ens
Chat "*"--"1" Coo
Mes "*"--"1" Chat
Mes "*"--"1" User
Sta "1"--"*" Ens
Sta "1"--"*" Chat
Sta "1"--"*" User
Sta "1"--"*" Emp
Sta "1"--"*" Eco
Mds "1"--"*" Sta
Mds "1"--"*" Emp
Mds "1"--"*" Chat
Eva "*"--"1" Sta
Eva "*"--"1" Mds
Eva "1"--"1" Ens
Hor "1"--"1" Sta
Hor "1"--"1" Mds
Plh "0..*"--"*" Hor
Coo -- Ens

```
