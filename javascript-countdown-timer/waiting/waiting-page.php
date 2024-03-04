<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
<script>
	function _notification(value)
	{	
		jQuery.ajax({
			url:'AJAX_GET_DATA.php',
			type:'post',
			data: {"notification":value},
			success:function(result)
			{
				document.getElementById(value).innerHTML = result;
			}
		});
	}
</script>



<!DOCTYPE HTML>
<html>
<head>
	<meta name="viewport" content="width=device-width, initial-scale=1">

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
</head>
<body>
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
		<h2>Waiting for opponent</h2>
		<p id="lazy"></p>  
	  </div>
	</div>
	
	
	<div id="dateTime">
		<?php
			// query
			echo "Sep 20, 2022 13:02:30"
		?>	
	</div>
	<div id="checkStatus"></div>


	<script>
	
	
	var tDate = document.getElementById("dateTime").innerText;

	// Set the date we're counting down to
	var countDownDate = new Date(tDate).getTime();

	// Update the count down every 1 second
	var x = setInterval(function() {
		
	  // Get today's date and time
	  var now = new Date().getTime();
		
	  // Find the distance between now and the count down date
	  var distance = (countDownDate + 120000) - now;
		
	  // Time calculations for days, hours, minutes and seconds
	  var days = Math.floor(distance / (1000 * 60 * 60 * 24));
	  var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
	  var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
	  var seconds = Math.floor((distance % (1000 * 60)) / 1000);
		
	  // Output the result in an element with id="countdown"
	  /*document.getElementById("countdown").innerHTML = days + "d " + hours + "h "
	  + minutes + "m " + seconds + "s ";*/
	  
	  
	  
	  document.getElementById("lazy").innerHTML = ("0" + minutes).slice(-2) + " : " + ("0" + seconds).slice(-2);
	  
	  // if opponent accept requert
	  _notification("checkStatus");
	  let cStatus = document.getElementById("checkStatus").innerHTML;
	  
	  if(cStatus==1)
	  {
		clearInterval(x);
		document.getElementById("lazy").innerHTML = "accept";
		window.location.assign("/home.php");
	  }
	  // if opponent reject requert
	  else if(cStatus==2)
	  {
		//clearInterval(x);
		document.getElementById("lazy").innerHTML = "reject";
		window.location.assign("/challenge-page.php");
	  }
	  // If the count down is over
	  else if (distance < 0) {
		clearInterval(x);
		document.getElementById("lazy").innerHTML = "EXPIRED";
	  }
	  
	},1000);
	
	</script>
</body>
</html>