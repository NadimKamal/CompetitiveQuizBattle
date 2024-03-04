<?php
    //require_once 'connection.php';
    
    
    
    function test_input($data)
    {
        $data = trim($data);
        $data = stripslashes($data);
        $data = htmlspecialchars($data);
        return $data;
    }
    
    if(isset($_POST['ChallengeYourFriend']))
    {
        session_start();
        $u_id = $_SESSION["user_id"];
        $category = test_input($_POST['category']);
        $class = test_input($_POST['class']);
        $classSubject = test_input($_POST['classSubject']);
        $email = test_input($_POST['email']);
        $opponent = ReturnOneColumnByAnotherColumnValue($email);
        if($opponent=="")
        {
            echo '<script>alert("Invalid Opponent")</script>';
            //sleep(3);
            //header('Location: QuizSections.php');
			//exit;
        }
        else{
        $sql =  "INSERT into CM_BATTLE_REQUEST (SENDER_PARTY,RECEPIENT_PARTY,REQUEST_TEXT,REQUEST_STATE) values('$u_id', '$opponent', '$classSubject','P')";
        ExecuteNonQuery($sql);
        //echo $sql;
        header('Location: waiting-page.php?dateTime=' . $_POST["dateTime"]);
        exit;
        }
        session_abort();
    }
    /*function dropdownType()
    {
        try{
        $get_data = "Select SYS_DEPT_TYPE_ID,SYS_DEPT_TYPE from CM_SYSTEM_DEPT_TYPE ";
		$conn = Openconnection();
		$qry = sqlsrv_query($conn,$get_data);
		while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
            {
                $key = $result['SYS_DEPT_TYPE_ID'];
                $val = $result['SYS_DEPT_TYPE'];
                echo "<option value='$key'>$val</option>";
            }
            sqlsrv_free_stmt($qry);
            sqlsrv_close($conn);
		} catch (\Throwable $th) {
            //echo $th;
			echo '<script>alert("Error")</script>';
		}
        return 1234;
    }*/

    /*function dropdownDept($id)
    {
        try{
        $get_data = "Select SYS_DEPT_ID,SYS_DEPT_NAME from CM_SYSTEM_DEPT WHERE SYS_DEPT_TYPE_ID='$id'";
		$conn = Openconnection();
		$qry = sqlsrv_query($conn,$get_data);
        $result="";
		while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
            {
                $key = $result['SYS_DEPT_ID'];
                $val = $result['SYS_DEPT_NAME'];
                $result=$result. "<option value='$key'>$val</option>";
            }
            sqlsrv_free_stmt($qry);
            sqlsrv_close($conn);
		} catch (\Throwable $th) {
			echo '<script>alert("Error")</script>';
		}
    }*/
    function ExecuteNonQuery($sql)
    {
        try {
			//$sql ="UPDATE CM_BATTLE_REQUEST SET REQUEST_STATE='A' WHERE REQUEST_ID='$notification_id'";
		//echo $sql;
		$conn = Openconnection();
		$qry = sqlsrv_query($conn,$sql);
		sqlsrv_free_stmt($qry);
        sqlsrv_close($conn);
		
		} catch (\Throwable $th) {
			echo "Error";
		}
    }
    function ReturnOneColumnByAnotherColumnValue($valueTwo)
    {
        try{
            $get_data = "Select ACCNT_ID from CM_ACCOUNT_LIST WHERE EMAIL='$valueTwo'";
            $conn = Openconnection();
            $qry = sqlsrv_query($conn,$get_data);
            $result="";
            $key="";
            while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
                {
                    $key = $result['ACCNT_ID'];
                    return $key;
                }
                sqlsrv_free_stmt($qry);
                sqlsrv_close($conn);
            } catch (\Throwable $th) {
                $key="";
                return $key;
            }
            return $key;
            
    }
    function ReturnNumberOfBattleWin($id)
    {
        $total=0;
        try{
            $get_data = "SELECT COUNT(*) CNT from CM_BATTLE_HISTORY where USER_ID='$id' AND USER_SCORE>OPPONENT_SCORE";
            $conn = Openconnection();
            $qry = sqlsrv_query($conn,$get_data);
            $result="";
            $key="";
            while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
                {
                    $key = $result['CNT'];
                    $total+=$key;
                }
                sqlsrv_free_stmt($qry);
                sqlsrv_close($conn);
            } catch (\Throwable $th) {
                
            }
            
            try{
                $get_data = "SELECT COUNT(*) CNT from CM_BATTLE_HISTORY where  USER_SCORE<OPPONENT_SCORE AND OPPONENT_ID='$id'";
                $conn = Openconnection();
                $qry = sqlsrv_query($conn,$get_data);
                $result="";
                $key="";
                while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
                    {
                        $key = $result['CNT'];
                        $total+=$key;
                        
                    }
                    sqlsrv_free_stmt($qry);
                    sqlsrv_close($conn);
                } catch (\Throwable $th) {
                    
                }
                return $total;
            
    }


    /*function dropdownTypeV2()
    {
        /*try{
        $get_data = "Select SYS_DEPT_TYPE_ID,SYS_DEPT_TYPE from CM_SYSTEM_DEPT_TYPE ";
		$conn = Openconnection();
		$qry = sqlsrv_query($conn,$get_data);
		while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
            {
                $key = $result['SYS_DEPT_TYPE_ID'];
                $val = $result['SYS_DEPT_TYPE'];
                echo "<option value='$key'>$val</option>";
            }
            sqlsrv_free_stmt($qry);
            sqlsrv_close($conn);
		} catch (\Throwable $th) {
            //echo $th;
			echo '<script>alert("Error")</script>';
		}

    }*/

    function UniqueRandomNumbersWithinRange($min, $max, $quantity) {
        $numbers = range($min, $max);
        shuffle($numbers);
        return array_slice($numbers, 0, $quantity);
    }
   
?>