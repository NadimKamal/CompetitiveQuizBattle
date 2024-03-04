<?php

require 'connection.php';
require_once 'php_function.php';

$var = 'http://'.$_SERVER['HTTP_HOST'].$_SERVER['REQUEST_URI'];
header("Refresh: 10; URL='$var'");

session_start();
$notification_id = $_SESSION['temp_notification'];

//echo $notification_id;

$own_id = $_SESSION["user_id"];
//echo $own_id;

    try{

      $usr_scr="";
      $opp_scr="";
      $usr_id="";
      $opp_id="";
      $get_data = "SELECT * FROM [QB_DB].[dbo].[CM_BATTLE_HISTORY]  WHERE REQUEST_ID='$notification_id'";
      $conn = Openconnection();
      $qry = sqlsrv_query($conn,$get_data);
      while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
          {
              $usr_scr = $result['USER_SCORE'];
              $opp_scr = $result['OPPONENT_SCORE'];
              $usr_id=$result['USER_ID'];
              $opp_id=$result['OPPONENT_ID'];
              
          }
          sqlsrv_free_stmt($qry);
          sqlsrv_close($conn);
      
      if($usr_scr=="" || $opp_scr=="")
      {
          
      }
      else{
      if($own_id==$opp_id)
      {
          if($opp_scr<$usr_scr)
          {
            try{
              $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$usr_id' WHERE REQUEST_ID='$notification_id'";
              ExecuteNonQuery($qry);
          }
          catch(Exception $e)
          {
              //echo $e;
          }
              ob_start();
              header('Location: loser-page.php');
              exit;
          }
          else if($opp_scr>$usr_scr)
          {
              try{
                $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$opp_id' WHERE REQUEST_ID='$notification_id'";
                ExecuteNonQuery($qry);
            }
            catch(Exception $e)
            {
                //echo $e;
            }
              ob_start();
              header('Location: winner-page.php');
              exit;
          }
          else {
              ob_start();
              header('Location: draw-page.php');
              exit;
          }
      }
      else
      {
          if($opp_scr>$usr_scr)
          {
            try{
              $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$usr_id' WHERE REQUEST_ID='$notification_id'";
              ExecuteNonQuery($qry);
          }
          catch(Exception $e)
          {
              //echo $e;
          }
              ob_start();
              header('Location: loser-page.php');
              exit;
          }
          else if($opp_scr<$usr_scr)
          {
            try{
              $qry = "UPDATE CM_BATTLE_HISTORY SET WINNER_ID='$opp_id' WHERE REQUEST_ID='$notification_id'";
              ExecuteNonQuery($qry);
          }
          catch(Exception $e)
          {
              //echo $e;
          }
              ob_start();
              header('Location: winner-page.php');
              exit;
          }
          else {
              ob_start();
              header('Location: draw-page.php');
              exit;
          }
      }
    }
}
catch(Exception $e)
{
    echo "xxxx";
}
    

?>

<style>
	@import url(https://fonts.googleapis.com/css?family=Yanone+Kaffeesatz:200,300,400);

	* {
	  box-sizing: border-box;
	}

	html {
	  background: #605B9B;
	  color: #fff;
	  font-family: 'Yanone Kaffeesatz', sans-serif;
	}

	body {
	  padding: 20px;
	}

	.timer-group
	{   
		height: 400px;
		margin: 10% auto;
		position: relative;
		width: 400px;
	}

	.timer {
	  border-radius: 50%;
	  height: 100px;
	  overflow: hidden;
	  position: absolute;
	  width: 100px;
	}

	.timer:after {
	  background: #111;
	  border-radius: 50%;
	  content: "";
	  display: block;
	  height: 80px;
	  left: 10px;
	  position: absolute;
	  width: 80px;
	  top: 10px;
	}

	.timer .hand {
	  float: left;
	  height: 100%;
	  overflow: hidden;
	  position: relative;
	  width: 50%;
	}

	.timer .hand span {
	  border: 50px solid rgba(0, 255, 255, .4);
	  border-bottom-color: transparent;
	  border-left-color: transparent;
	  border-radius: 50%;
	  display: block;
	  height: 0;
	  position: absolute;
	  right: 0;
	  top: 0;
	  transform: rotate(225deg);
	  width: 0;
	}

	.timer .hand:first-child {
	  transform: rotate(180deg);
	}

	.timer .hand span {
	  animation-duration: 4s;
	  animation-iteration-count: infinite;
	  animation-timing-function: linear;
	}

	.timer .hand:first-child span {
	  animation-name: spin1;
	}

	.timer .hand:last-child span {
	  animation-name: spin2; 
	}

	.timer.hour {
	  background: rgba(0, 0, 0, .3);
	  height: 400px;
	  left: 0;
	  width: 400px;
	  top: 0;
	}

	.timer.hour .hand span {
	  animation-duration: 3600s;
	  border-top-color: rgba(255, 0, 255, .4);
	  border-right-color: rgba(255, 0, 255, .4);
	  border-width: 200px;
	}

	.timer.hour:after {
	  height: 360px;
	  left: 20px;
	  width: 360px;
	  top: 20px;
	}

	.timer.minute {
	  background: rgba(0, 0, 0, .2);
	  height: 350px;
	  left: 25px;
	  width: 350px;
	  top: 25px;
	}

	.timer.minute .hand span {
	  animation-duration: 120s;
	  border-top-color: rgba(255, 255, 255, .99);
	  border-right-color: rgba(255, 255, 255, .99);
	  border-width: 175px;
	}

	.timer.minute:after {
	  height: 310px;
	  left: 20px;
	  width: 310px;
	  top: 20px;
	}

	.timer.second {
	  background: rgba(0, 0, 0, .2);
	  height: 300px;
	  left: 50px;
	  width: 300px;
	  top: 50px;
	}

	.timer.second .hand span {
	  animation-duration: 1s;
	  border-top-color: rgba(255, 255, 255, .25);
	  border-right-color: rgba(255, 255, 255, .25);
	  border-width: 150px;
	}

	.timer.second:after {
	  height: 296px;
	  left: 2px;
	  width: 296px;
	  top: 2px;
	}

	.face {
	  background: rgba(0, 0, 0, .1);
	  border-radius: 50%;
	  height: 296px;
	  left: 52px;
	  padding: 165px 40px 0;
	  position: absolute;
	  width: 296px;
	  text-align: center;
	  top: 52px;
	}

	.face h2 {
	  font-weight: 300; 
	}

	.face p
	{
	  border-radius: 20px;
	  font-size: 76px;
	  font-weight: 400;
	  position: absolute;
	  top: 17px;
	  width: 260px;
	  left: 20px;
	}

	@keyframes spin1 {
	  0% {
		transform: rotate(225deg);
	  }
	  50% {
		transform: rotate(225deg);
	  }
	  100% {
		transform: rotate(405deg);
	  }
	}

	@keyframes spin2 {
	  0% {
		transform: rotate(225deg);
	  }
	  50% {
		transform: rotate(405deg);
	  }
	  100% {
		transform: rotate(405deg);
	  }
	}
	</style>

<div class="timer-group">
  <!--<div class="timer hour">
    <div class="hand"><span></span></div>
    <div class="hand"><span></span></div>
  </div>-->
  <div class="timer minute">
    <div class="hand"><span></span></div>
    <div class="hand"><span></span></div>
  </div>
  <div class="timer second">
    <div class="hand"><span></span></div>
    <div class="hand"><span></span></div>
  </div>
  <div class="face">
    <h2>Wait for opponent to finish battle</h2>
    <p id="lazy">00:00:00</p>  
  </div>
</div>

<script>

var defaults = {}
  , one_second = 1000
  , one_minute = one_second * 60
  , one_hour = one_minute * 60
  , one_day = one_hour * 24
  , startDate = new Date()
  , face = document.getElementById('lazy');

// http://paulirish.com/2011/requestanimationframe-for-smart-animating/
var requestAnimationFrame = (function() {
  return window.requestAnimationFrame       || 
         window.webkitRequestAnimationFrame || 
         window.mozRequestAnimationFrame    || 
         window.oRequestAnimationFrame      || 
         window.msRequestAnimationFrame     || 
         function( callback ){
           window.setTimeout(callback, 1000 / 60);
         };
}());

tick();

function tick() {

  var now = new Date()
    , elapsed = now - startDate
    , parts = [];

  parts[0] = '' + Math.floor( elapsed / one_hour );
  parts[1] = '' + Math.floor( (elapsed % one_hour) / one_minute );
  parts[2] = '' + Math.floor( ( (elapsed % one_hour) % one_minute ) / one_second );

  parts[0] = (parts[0].length == 1) ? '0' + parts[0] : parts[0];
  parts[1] = (parts[1].length == 1) ? '0' + parts[1] : parts[1];
  parts[2] = (parts[2].length == 1) ? '0' + parts[2] : parts[2];

  face.innerText = parts.join(':');
  
  requestAnimationFrame(tick);
  
}

</script>