<?php
require 'php_function.php';
require_once 'connection.php';

    if(isset($_POST['keyValue']))
    {
        // this is for class and dept
        if($_POST['keyValue'][0]=="category")
        {
            $function = "\"" . "_select('class','subjectOption')" . "\"";
            $v = "<select id='class' name='class' onchange=$function><option value=''>Select</option>";
            
                //$key = $i;
                $value = $_POST['keyValue'][1];
                try{
                    $get_data = "Select SYS_DEPT_ID,SYS_DEPT_NAME from CM_SYSTEM_DEPT WHERE SYS_DEPT_TYPE_ID='$value'";
                    $conn = Openconnection();
                    $qry = sqlsrv_query($conn,$get_data);
                    $result="";
                    while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
                        {
                            $key = $result['SYS_DEPT_ID'];
                            $val = $result['SYS_DEPT_NAME'];
                            $v = $v . "<option value='$key'>$val</option>";
                        }
                        sqlsrv_free_stmt($qry);
                        sqlsrv_close($conn);
                    } catch (\Throwable $th) {
                        echo '<script>alert("Error")</script>';
                    }

            $v = $v . "</select>";
            echo $v;
        }
        else if($_POST['keyValue'][0]=="class")
        {
            $v = "<select id='classSubject' name='classSubject'><option value=''>Select</option>";
            
            $value = $_POST['keyValue'][1];

            try{
                $get_data = "Select SYS_SUB_ID,SYS_SUB_NAME from CM_SYSTEM_SUBJECT WHERE SYS_DEPT_ID='$value'";
               
                
                $conn = Openconnection();
                $qry = sqlsrv_query($conn,$get_data);
                $result="";
                while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
                    {
                        $key = $result['SYS_SUB_ID'];
                        $val = $result['SYS_SUB_NAME'];
                        $v = $v . "<option value='$key'>$val</option>";
                    }
                    sqlsrv_free_stmt($qry);
                    sqlsrv_close($conn);
                } catch (\Throwable $th) {
                    echo '<script>alert("Error")</script>';
                }

        $v = $v . "</select>";
        echo $v;
        }
        
        
    }

    if(isset($_POST["notification"]))
	{
		if($_POST["notification"]=="checkStatus")
		{
			// query
            session_start();
            $id = $_SESSION["user_id"];
			//$id = 220918000001;
			try
			{
				$get_data = "SELECT REQUEST_STATE FROM [QB_DB].[dbo].[CM_BATTLE_REQUEST] WHERE SENDER_PARTY='$id' and REQUEST_DATE=(SELECT MAX(REQUEST_DATE) FROM [QB_DB].[dbo].[CM_BATTLE_REQUEST] WHERE SENDER_PARTY='$id')";
				//echo $get_data;
				$conn = Openconnection();
				$qry = sqlsrv_query($conn,$get_data);
				$result="";
				//echo $qry;
				while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
				{
					echo $result['REQUEST_STATE'];
					//echo $result . "  jdfigjer f";
				}
				sqlsrv_free_stmt($qry);
				sqlsrv_close($conn);
			}
			catch (\Throwable $th)
			{
				//echo $th;
				echo '<script>alert("Error")</script>';
			}
			
			// print status
			//echo 21;
		}
	}
?>