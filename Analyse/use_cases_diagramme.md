# Cas d'utilisations

```plantuml
left to right direction
actor Coordonateur as a1
actor Enseignant as a2
actor Stagiaire as a3
actor MDS as a4
actor Entreprise as a5

package Application_SPU {
  usecase "Consulter horaire" as UC1
  usecase "Modifier horaire" as UC2
  usecase "Entrer son horaire" as UC3
  usecase "Confirmer présence/absence dans l'horaire" as UC4
  usecase "Faire l'auto-évaluation" as UC5
  usecase "Faire l'évaluation" as UC6
  usecase "Consulter l'auto-évaluation" as UC7
  usecase "Envoyer message CHAT" as UC8
  usecase "Accéder au CHAT" as UC9
  usecase "Assignation MDS et stagiaire" as UC10
  usecase "Assignation Enseignant et Stagiaire" as UC11
  usecase "Accéder auto-évaluation et évaluations" as UC12
  usecase "Signer contrat de stage" as UC13
  usecase "Ajouter un compte" as UC14
  usecase "Modifier un compte" as UC15
  usecase "Supprimer un compte" as UC16
  usecase "Exporter formulaire d'accompagnement" as UC17

  usecase "Logout" as UC20
  usecase "Login (tout le monde)" as UC21
}

a2 ----------> UC1
UC1 ...> UC20 : <<Include>>
a3 ----------> UC1
a4 ----------> UC1
a5 ----------> UC1

a4 ----------> UC2
UC2 ...> UC20 : <<Include>>

a4 ----------> UC3
UC3 ...> UC20 : <<Include>>

a3 ----------> UC4
a4 ----------> UC4

a3 ----------> UC5

a4 ----------> UC6

a3 ----------> UC7
a2 ----------> UC7

a2 ----------> UC8
a3 ----------> UC8
a4 ----------> UC8
UC8 ...> UC20 : <<Include>>

a2 ----------> UC9
a3 ----------> UC9
a4 ----------> UC9
UC9 ...> UC20 : <<Include>>

a1 ----------> UC10
a1 ----------> UC11

a2 ----------> UC12  : Excepter le coordonateur

a1 ----------> UC13

a2 ----------> UC14
a3 ----------> UC14
a4 ----------> UC14
UC14 ...> UC20 : <<Include>>

a1 --> UC15
UC15 ...> UC20 : <<Include>>

a1 --> UC16
UC16 ...> UC20 : <<Include>>

a1 --> UC17

a2 <|- a1
a3 <|- a1
a4 <|- a1
a5 <|- a1


```