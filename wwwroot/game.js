﻿let turnTimer; // Global timer-referens
let turnTimeLeft = 60; // Initialt antal sekunder
let currentPlayer = 1; // sätt nuvarande spelare till 1
let isCurrentPlayerTurn = true;  // Sätt nuarande spelare till sant
let previousShotCount = 0;  // gör en tidigare shotcount för att jämföra med.

let shotCountTimer;

const connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

const gameInfoElement = document.getElementById("game-info");
const gameIdString = gameInfoElement ? gameInfoElement.dataset.gameId : null;
const gameId = gameIdString ? parseInt(gameIdString, 10) : null;

if (gameId== null) {
    console.error("Game ID saknas. Kontrollera att game-info-diven innehåller data-game-id-attributet.");
} else {
    console.log(`Game ID hämtat: ${gameId}`);
}

connection.start()
    .then(() => {        
        //startSimpleTimer();
        //startShotCountCheck(); // Starta skottkontrollen
        connection.invoke("JoinLobby", gameIdString) 
            .then(() => console.log("Shot count checked."))
            .catch(err => console.error("Error checking shot count:", err));
    }).catch(err => {
        console.error("Kunde inte starta SignalR-anslutningen:", err);
    });

connection.on("UpdatePlayerList", function (players) {
    const playerList = document.getElementById("playerList");
    playerList.innerHTML = ""; // Töm listan

    players.forEach(player => {
        const li = document.createElement("li");
        li.textContent = player;
        playerList.appendChild(li);
    });
    // Aktivera "Starta spelet"-knappen om det finns minst två spelare
    //document.getElementById("startGameBtn").disabled = players.length !== 2;
    if (players.length == 2) {
        startShotCountCheck();
        startSimpleTimer();
    }

});


function startSimpleTimer() {
    if (turnTimer) {
        clearInterval(turnTimer);
    }

    turnTimeLeft = 60; 
    updateTimerDisplay(turnTimeLeft); 

    turnTimer = setInterval(() => {
        turnTimeLeft -= 1;
        updateTimerDisplay(turnTimeLeft);

        if (turnTimeLeft <= 0) {
            clearInterval(turnTimer); // Stoppa timern
            document.getElementById("message").textContent = `Tiden är ute! Spelare ${currentPlayer} förlorade.`;

            // Deklarera den andra spelaren som vinnare
            declareWinner(currentPlayer === 1 ? 2 : 1);
        }

    }, 1000); // 1000 ms = 1 sekund
}

function startShotCountCheck() {
    shotCountTimer = setInterval(() => {
        if (connection.state === signalR.HubConnectionState.Connected) {
            console.log("Kontrollerar antalet skott...");

                connection.invoke("CheckShotCountChange",gameId).catch(err => console.error("Fel vid kontroll av skottantal:", err));
        } else {
            console.warn("Anslutningen är inte redo.");

        }
    }, 5000);  // Kontrollera var femte sekund
}
function updateTimerDisplay(timeLeft) {
    const timerElement = document.getElementById("timer");
    if (timerElement) {
        timerElement.textContent = `Tid kvar: ${timeLeft} sekunder`;
    } else {
        console.error("Timer-elementet hittades inte!");
    }
}


function declareWinner(winnerPlayer) {
    const numericGameId = parseInt(gameId, 10);
    if (isNaN(numericGameId)) {
        console.error("Ogiltigt gameId-värde.");
        return;
    }

    console.log(`Declaring winner player ${winnerPlayer} for game ${numericGameId}`);
    connection.invoke("DeclareWinner", numericGameId, winnerPlayer)
        .then(() => {
            console.log(`Winner declared: Player ${winnerPlayer}`);
        })
        .catch(err => {
            console.error("Error invoking DeclareWinner:", err.toString());
            alert("Kunde inte deklarera vinnaren. Försök igen senare.");
        });

    window.location.href = `/BattleField2/Result?gameId=${numericGameId}`

}

connection.on("ShotCountChanged", (newShotCount) => {
    console.log(`Antal skott uppdaterat: ${newShotCount}`);

    if (newShotCount !== previousShotCount) {
        console.log(`Skottantal ändrades! Föregående: ${previousShotCount}, Nytt: ${newShotCount}`);
        previousShotCount = newShotCount;  // Uppdatera det föregående värdet

        if (isCurrentPlayerTurn) {
            startSimpleTimer();
        }
    } else {
        console.log("Inget skottantal förändrat.");
    }
});


connection.invoke("CheckShotCountChange", gameId)
    .then(() => console.log("Shot count checked.")).catch(err => {
        
    });


function RedirectResult(gameId) {
    window.location.href = `/BattleField2/Result?gameId=${gameId}`

}









