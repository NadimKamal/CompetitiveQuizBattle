<?php
    require('connection.php');
    session_start();
    $u_id = $_SESSION["user_id"];
   
    
    
    try{
		//$login_email = $_POST["login-email"];
		//$pass = $_POST["login-pass"];
		$get_data =  "SELECT CR.*,AL.ACCNT_NAME,SS.SYS_SUB_NAME from CM_BATTLE_REQUEST CR,CM_ACCOUNT_LIST AL,CM_SYSTEM_SUBJECT SS where AL.ACCNT_ID=CR.SENDER_PARTY AND CR.REQUEST_TEXT=SS.SYS_SUB_ID AND RECEPIENT_PARTY='$u_id' and REQUEST_STATE='P'";
        //echo $get_data;
		$conn = Openconnection();
		$qry = sqlsrv_query($conn,$get_data);
        //$row_count = sqlsrv_num_rows( $qry );
        $row_count=0;
        
		while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
            {
                
				
                $id = $result['REQUEST_ID'];
                $temp=$result['ACCNT_NAME'];
                $subName=$result['SYS_SUB_NAME'];
                $final=$temp." Challenged You For ".$subName;
                
                echo "<li>".$final."
                    <br />
                    <form method='post'>
                        <input type = 'text' name='notification_id' value='$id' style = 'display: none'/>
                        <input type='submit' value='Accept' name='ChallangeAccept' />
                        <input type='submit' value='Reject' name='ChallangeReject' style='background-color: red; float: right' />
                    </form>
                    </li>";
                //echo "<br>";
                $row_count++;
            }
            echo "<div id='nCount' style='display:none;'>$row_count</div>";
            sqlsrv_free_stmt($qry);
            sqlsrv_close($conn);
		} catch (\Throwable $th) {
			//echo '<script>alert("Invalid UserId or Password")</script>';
		}
    
    
    
    
    
    //echo $get_data;
?>