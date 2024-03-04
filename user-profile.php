<?php
    require 'user-header.php';
    
    
?>
<style>
    .mainContainer
    {
        padding:100px 0;
        background: rgb(2,0,36);
        background: linear-gradient(176deg, rgba(2,0,36,1) 0%, rgba(16,79,103,1) 0%, rgba(0,212,255,1) 100%);
        height:1025px;
    }
    .subContainer
    {
        margin:auto!important;
        width:86%!important;
        text-align:left;
        clear:both;
    }
    .tableCover
    {
        max-height: 300px;
        width: 100%;
        border: 1px solid #4CAF50;
        overflow-y: scroll;
        overflow-x: hidden;
    }
</style>
<div class="mainContainer">
	<style>
		.aboutUserInfo
		{
			width:25%;
			//border:1px solid black;
			min-height:500px;
			box-shadow: rgba(0, 0, 0, 0.12) 0px 1px 3px, rgba(0, 0, 0, 0.24) 0px 1px 2px;
			padding:2px;
			margin-top:10px;
			//background:#F0F8FF;
			float:left;
		}
		
		.aboutUserInfo #userPhoto
		{
			width:60%;
			margin:5% auto;
		}
		.aboutUserInfo img
		{
			width:100%;
		}
		
		.aboutUserInfo .oinfo
		{
			background:#2F4F4F;
			color:#F0F8FF;
			font-size:16px;
			width:;
			padding:1%;
			padding-left:3%;
			-webkit-border-top-right-radius: 15px;
			-moz-border-radius-topright: 15px;
			border-top-right-radius: 15px;
			margin-bottom:1%;
		}
		
		.aboutUserChalanges
		{
			width:73%;
			//background:#cccccc;
			height:111px;
			float:right;
			margin-top:10px;
		}
		.scoreAndtotalWon
		{
			width:25%;
			min-height:70px;
			float:left;
			background:green;
			color:#ffffff;
			font-size:20px;
			padding:10px;
			text-align:center;
		}
		.submitan
		{
			text-align: center;
			font-family: 'Roboto Slab', serif;
			margin-top: 10%;
		}

		.submitan a
		{
			color: white;
			background-color: #85144B;
			padding: 5px 11px;
			border-style: 2px solid
		}

		.submitan a:hover
		{
			border:1px solid #ffffff;
		}
		.submitancr a:hover
		{
			border:1px solid #ffffff;
		}
	</style>
	<div class="subContainer">
		<div class="aboutUserInfo">
			<?php
				
				try{
					$get_data = "SELECT ACCNT_NAME,EMAIL,INSTITUTE_NAME from CM_ACCOUNT_LIST where EMAIL='" . $_SESSION['user_email'] . "'";
					$conn = Openconnection();
					$qry = sqlsrv_query($conn,$get_data);
					$result="";
					$name;
					$email="";
					$ins="";
					while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
						{
							$name = $result['ACCNT_NAME'];
							$email = $result['EMAIL'];
							$ins = $result['INSTITUTE_NAME'];
						}
						sqlsrv_free_stmt($qry);
						sqlsrv_close($conn);
					} catch (\Throwable $th) {
						
					}
			?>
			
			<div id="userPhoto">
				<img src="https://www.pikpng.com/pngl/b/243-2439658_customer-service-icon-png-transparent-customer-icon-png.png"  name="userPhoto" />
			</div>
			
			<div class="oinfo" style="width:75%;"><?php echo $name ?></div>
			<div class="oinfo" style="width:85%;"><?php echo $email ?> </div>
			<div class="oinfo"><?php echo $ins ?> </div>
			<div class="submitan">
				<a href='edit_user_profile.php'>Edit Profile</a>
			</div>   
			
			
		</div>
		<div class="aboutUserChalanges">
			<div class="scoreAndtotalWon" style="background:#FF851B;">
				<div>Rank</div>
				<?php

	 			?>
				
				
			</div>
			<div class="scoreAndtotalWon" style="background:#85144B;">
				<div>Total No. of Battle</div>
				
				 <?php
					
						

					$idd = $_SESSION['user_id'];
					$ins="";
					
					 try{
						$get_data = "SELECT COUNT(*) CNT from CM_BATTLE_HISTORY where USER_ID='" . $_SESSION['user_id'] . "' OR OPPONENT_ID='" . $_SESSION['user_id'] . "'";
						$conn = Openconnection();
						$qry = sqlsrv_query($conn,$get_data);
						$result="";
						$name;
						$email="";
						$ins="";
						while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
							{
								$ins = $result['CNT'];
							}
							sqlsrv_free_stmt($qry);
							sqlsrv_close($conn);
						} catch (\Throwable $th) {
							
						}
					 
					 echo "<span style='font-size:26px;'>" . $ins . "</span>";
					
					
				?>
			</div>
			 <div class="scoreAndtotalWon" style="background:#B10DC9;">
				<div>No. of Win</div>
				<?php 
					
					echo "<span style='font-size:26px;'>" . ReturnNumberOfBattleWin($idd) . "</span>";
				?>
			</div>
			 <div class="scoreAndtotalWon" style="background:#001F3F;">
				<div>Win Percentage</div>
				<?php
					$win=ReturnNumberOfBattleWin($idd);
					$noOfp = $win/($ins) * 100;
					echo "<span style='font-size:26px;'>" . $noOfp  . " %</span>";
				?>
			</div>
			
			
			
			
			<style>
		#ranklist {
		  font-family: Arial, Helvetica, sans-serif;
		  border-collapse: collapse;
		  width: 100%;
		  color:white;
		  box-shadow: rgba(50, 50, 93, 0.25) 0px 6px 12px -2px, rgba(0, 0, 0, 0.3) 0px 3px 7px -3px;
		}
		.tableHwidth
		{
			width:27%;
			text-align:center;
		}
		#ranklist td, #ranklist th {
		  border: 1px solid #ddd;
		  padding: 8px;
		}
		
		#ranklist tr:nth-child(even){//background-color: #f2f2f2;}
		
		#ranklist tr:hover {background: -webkit-linear-gradient(left, #25c481, #25b7c4);
background: linear-gradient(to right, #25c481, #25b7c4);}
		
		#ranklist th {
		  padding-top: 12px;
		  padding-bottom: 12px;
		  text-align: left;
		  background-color: #04AA6D;
		  color: white;
		  background: -webkit-linear-gradient(left, #25c481, #25b7c4);
background: linear-gradient(to right, #25c481, #25b7c4);
		}
	</style>
	<br /><br /><br /><br /><br />
	<h2 style="text-align:center;">Battle History</h2>
	<br />
	       <div class="tableCover">
			<table id="ranklist">
			  <tr>
				<th class="tableHwidth">Opponent's Name</th>
				<th class="tableHwidth">Opponent's Score</th>
				<th>My Score</th>
				<th>Winner</th>
			  </tr>
			  <?php
			 
					
			 try{
				$get_data = "SELECT AL.ACCNT_NAME OPP,CH.OPPONENT_SCORE OPSC,CH.USER_SCORE MSC,WAL.ACCNT_NAME WIN FROM CM_BATTLE_HISTORY CH,CM_ACCOUNT_LIST AL,CM_ACCOUNT_LIST WAL WHERE CH.USER_ID='" . $_SESSION['user_id'] . "' AND CH.OPPONENT_ID=AL.ACCNT_ID AND WINNER_ID=WAL.ACCNT_ID UNION SELECT  AL.ACCNT_NAME OPP,CH.OPPONENT_SCORE MSC,CH.USER_SCORE OPSC,WAL.ACCNT_NAME WIN FROM CM_BATTLE_HISTORY CH,CM_ACCOUNT_LIST AL,CM_ACCOUNT_LIST WAL WHERE CH.USER_ID=AL.ACCNT_ID AND CH.OPPONENT_ID='" . $_SESSION['user_id'] . "' AND WINNER_ID=WAL.ACCNT_ID";
				$conn = Openconnection();
				$qry = sqlsrv_query($conn,$get_data);
				$result="";
				$name;
				$email="";
				$ins="";
				while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
					{
						$opponent_name = $result['OPP'];
						$opponent_score = $result['OPSC'];
						$user_score = $result['MSC'];
						$win = $result['WIN'];
						if($opponent_name!=""){
							echo "<tr>
							  <td class='rank'>$opponent_name</td>
							  <td>$opponent_score</td>
							  <td>$user_score</td>
							  <td>$win</td>
							</tr>";
							}
					}
					sqlsrv_free_stmt($qry);
					sqlsrv_close($conn);
				} catch (\Throwable $th) {
					
				}
				
			  ?>
			</table>
			</div>
		
		
			<style>
				#CreateTournament
				{
					color:black;
					background:white;
					padding:2px 8px;
					margin-bottom: 10px;
				}
			</style>
			<br>
			<h2 style="text-align:center;">Private Tournament</h2>
			<div class="submitancr" style="text-align: center; margin-top: 10px;font-family: 'Roboto Slab', serif;">
				<a style="color: white;background-color: #85144B;border-style: 2px solid" href="CreateTournament.php" id="CreateTournament">Create Tounament</a>
			</div>
			<br />
			<div class="tableCover">
    			<table id="ranklist">
    				  <tr>
    					<th class="tableHwidth">Id</th>
    					<th class="tableHwidth">Name</th>
    					<th>Start Time</th>
    					<th>End Time</th>
    					<th>Rank List</th>
    				  </tr>
    				  <?php
    				 /* try{
    					$qry = "SELECT ct.* FROM CreateTournament ct WHERE ct.created_by='".$user_id."' UNION SELECT ct.* from CreateTournament ct, private_result pr where pr.private_id=ct.private_id and pr.user_id='".$user_id."'";
    					$check = $conn->prepare($qry);
    					$check->execute();
    					$check_cnt = $check->rowCount();
    					$results = $check->fetchAll();
    				   
    					for($i=0;$i<$check_cnt;$i++){
    						$id = $results[$i][0];
    						$name = $results[$i][1];
    						$startTime = $results[$i][3];
    						$endTime = $results[$i][4];
    						$rank = $results[$i][0];
    						
    					  echo "<tr>
    						<td>$id</td>
    						<td>$name</td>
    						<td>$startTime</td>
    						<td>$endTime</td>
    						<td>$rank</td>
    					  </tr>";
    					}
    				  }
    				   catch(Exception $e)
    				  {
    					  echo $qry;
    				  }*/
    				  ?>
    			  <div style="clear: both"></div>
    			</table>
			</div>
		</div>
	</div>
	<div style="clear: both"></div>
	<br>
</div>
    
<?php
    require 'user-footer.php';
?> 