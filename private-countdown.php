<?php
    require 'user-header.php';
    session_start();
    $user_id = $_SESSION["user_id"];
    $id = $_GET["id"];
    
   
    //echo $qry;
    try{
        $qry = "SELECT start_time FROM CreateTournament WHERE private_id=".$id."";
        $check = $conn->prepare($qry);
        $check->execute();
        $check_cnt = $check->rowCount();
        $results = $check->fetchAll();
        
        $con_end = $results[0][0];
        
        //echo $con_end;
        
        echo "<div id='end_date' style='display: none'>$con_end</div>";
        echo "<div id='id' style='display: none'>?id=$id</div>";
        
    
    }
    catch(Exception $e)
    {
        header('Location: home.php');
		exit;
    }
        
    
?>
    
<!-- ***** Search Area ***** -->

<!-- ***** Main Banner Area Start ***** -->
<div class="main-banner header-text" id="top">
    <div class="Modern-Slider">
      <!-- Item -->
      <div class="item">
        <div class="img-fill">
            <img src="assets/images/slide-03.jpg" alt="">
            <style>
                .text-content
                {
                    top:30%!important;
                    //left:50%!important;
                    margin:auto!important;
                    //background:white;
                    width:80%!important;
                    text-align:left;
                }
            </style>
            <div class="text-content">
<style>
    .countdown {
      text-align: center;
      font-size: 100px;
      margin-top: 0px;
      color:black;
    }
    </style>
	<div class="main">

<p class="countdown" id="demo"></p>

<script>
// Set the date we're counting down to


// Update the count down every 1 second
var x = setInterval(function() {

  // Get today's date and time
  var end = document.getElementById("end_date").innerHTML;
  var countDownDate = new Date(end).getTime();
  
  
  
  var now = new Date().getTime();
  //alert(end);
    
  // Find the distance between now and the count down date
  var distance = countDownDate - now;
    
  // Time calculations for days, hours, minutes and seconds
  var days = Math.floor(distance / (1000 * 60 * 60 * 24));
  var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
  var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
  var seconds = Math.floor((distance % (1000 * 60)) / 1000);
    
  // Output the result in an element with id="demo"
  document.getElementById("demo").innerHTML = days + "d " + hours + "h "
  + minutes + "m " + seconds + "s ";
    
  // If the count down is over, write some text 
  if (distance < 0) {
    clearInterval(x);
    document.getElementById("demo").innerHTML = "STARTED";
    
    var id = document.getElementById("id").innerHTML;
	var link = "#";
    window.location.replace("./private-question-page.php"+id);

  } 
}, 1000);
</script>
	</div>
	</div>
	</div>
<!-- ***** Main Banner Area End ***** -->

<?php
    require 'user-footer.php';
?>