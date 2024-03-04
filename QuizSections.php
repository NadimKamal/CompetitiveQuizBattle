<?php
    require 'user-header.php';
    //require 'php_function.php';
    //require 'connection.php';
    
?>
<!-- ***** Main Banner Area Start ***** -->
<style>
.text-content
{
   top:55%!important;
    //left:50%!important;
    margin:auto!important;
    //background:white;
    width:80%!important;
    text-align:left;
}
input[type=text],input[type=email], select, textarea {
  width: 100%;
  padding: 12px;
  border: 1px solid #ccc;
  border-radius: 4px;
  resize: vertical;
  color:#7d6ae6;
}

label {
  padding: 12px 12px 12px 0;
  display: inline-block;
  color:#fff;
}

input[type=submit] {
  background-color: #fff;
  color: #685DA2;
  padding: 12px 40px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  float: right;
    font-weight: bold;

}

input[type=submit]:hover {
  //background-color: #45a049;
}

.col-25 {
  float: left;
  width: 25%;
  margin-top: 6px;
}

.col-75 {
  float: left;
  width: 75%;
  margin-top: 6px;
}

/* Clear floats after the columns */
.row:after {
  content: "";
  display: table;
  clear: both;
}

/* Responsive layout - when the screen is less than 600px wide, make the two columns stack on top of each other instead of next to each other */
@media screen and (max-width: 600px) {
  .col-25, .col-75, input[type=submit] {
    width: 100%;
    margin-top: 0;
  }
}
</style>
<div class="main-banner header-text" id="top">
    <div class="Modern-Slider">
      <!-- Item -->
      <div class="item">
        <div class="img-fill">
            <img src="assets/images/slide-03.jpg" alt="">
            <div class="text-content">
                <h3 style="text-align:center;font-size:25px;">Challenge Your Friends</h3>
                <br />
                <form method="post" action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]);?>">
                	<div class="row">
                	<div class="col-25">
                	  <label for="Category">Select Category</label>
                	</div>
                	<div class="col-75">
                	  <select id="category" name="category" onchange="_select('category','classOption')">
                		<option value="">Select</option>
                		<?php 
                    
                        function dropdownTypeV2()
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
                        }
                        dropdownTypeV2();
                    
                    ?>
                	  </select>
                	</div>
                	</div>
                	<div class="row">
                	<div class="col-25">
                	  <label for="class">Select Class or Department</label>
                	</div>
                	<div class="col-75">
                	    <div id="classOption">
                        <select id='class' name='class' onchange="_select('class','subjectOption')">
                        </select>
                      </div>
                	</div>
                	</div>
                	<div class="row">
                	<div class="col-25">
                	  <label for="country">Select Subject</label>
                	</div>
                	<div class="col-75">
                	    <div id="subjectOption">
                    	  <select id="classSubject" name="classSubject">
                    	  </select>
                	    </div>
                	</div>
                    </div>
                	<div class="row">
                	<div class="col-25">
                	  <label for="email">Opponent's Email Address</label>
                	</div>
                	<div class="col-75">
                	  <input type="email" id="email" name="email" placeholder="Opponent's Email Address">
                	</div>
                	</div>
                  <input type="text" id="dateTime" name="dateTime" style="display:none;">
                  <script>
                      function myDateTime()
                      {
                        document.getElementById("dateTime").value = (new Date().getTime()); 
                      }
                  </script>
                	<br>
                	<div class="row" style="float:right;">
                		<input type="submit" value="Submit" name="ChallengeYourFriend" onclick="myDateTime()">
                	</div>
                </form>
            </div>
        </div>
      </div>
      <!-- // Item -->
    </div>
</div>
<!-- ***** Main Banner Area End ***** -->

<?php
    require 'user-footer.php';
?>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
<script>

  
function _select(type,htmlId)
{
    let v = document.getElementById(type).value;
    let k = type;
    //alert(v + " - " +k);
    if(v!="")
    {
    	jQuery.ajax({
    		url:'AJAX_GET_DATA.php',
    		type:'post',
    		data: {"keyValue":[k,v]},
    		success:function(result)
    		{
    			document.getElementById(htmlId).innerHTML = result;
    		}
    	});
    }
}
</script>