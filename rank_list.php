<?php
    require 'user-header.php';
    
    
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
                    #ranklist {
                      font-family: Arial, Helvetica, sans-serif;
                      border-collapse: collapse;
                      width: 100%;
                      color:white;
                      box-shadow: rgba(50, 50, 93, 0.25) 0px 6px 12px -2px, rgba(0, 0, 0, 0.3) 0px 3px 7px -3px;
                    }
                    .rank
                    {
                        width:25px;
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
                    
                    #searchData
                    {
                        margin-top:40px;
                    }
                    #searchData option
                    {
                        color:black;
                        min-width:250px;
                    }
                    #searchData label
                    {
                        color:white;
                    }
                    #searchData #InstituteName
                    {
                        width:150px;
                        padding:3px;
                    }
                </style>
                
                <div id="searchData">
                    <!-- Start -->
                    <form class="searchForm" action="" method="get">
                        <label for="InstituteName">Select Institute:</label>
                        <select name="InstituteName" id="InstituteName">
                            <option value="">---Select---</option>
                            <?php
                               
                               
                                $get_data = "SELECT * FROM CM_ACCOUNT_LIST WHERE GROUP BY INSTITUTE_NAME";
                                $conn = Openconnection();
                                $qry = sqlsrv_query($conn,$get_data);
                                $result="";
                                $name="";
                                $email="";
                                $ins="";
                                $pass="";
                                while($result = sqlsrv_fetch_array($qry, SQLSRV_FETCH_ASSOC))
                                  {
                                   
                                    $ins=$result['INSTITUTE_NAME'];
                                   
							?>
									<option value="<?php echo $ins;?>"> <?php echo $ins; ?> </option>
							<?php
								}
                sqlsrv_free_stmt($qry);
                sqlsrv_close($conn);
                            ?>
                        </select>
                        <input type="submit" value="Submit" name="ins" />
                    </form>
                <!-- End -->
                </div>
                
                <table id="ranklist">
                
                  <tr>
                    <th class='rank'>Rank</th>
                    <th>Name</th>
                    <th>Institute</th>
                    <th>Points</th>
                  </tr>
                  <?php
                    if(isset($_GET["ins"]))
                    {
                        $qry = "SELECT ut.name,ut.institute_name,COUNT(*) FROM Notifications nt,UserTable ut WHERE flag=1 and winner_id is not null and nt.winner_id=ut.id AND ut.institute_name='" . $_GET['InstituteName'] . "' GROUP BY winner_id ORDER BY COUNT(*) DESC";
                        $check = $conn->prepare($qry);
                        $check->execute();
                        $check_cnt = $check->rowCount();
                        $results = $check->fetchAll();
                        $one = 1;
                        for($i=0;$i<$check_cnt;$i++){
                            $name = $results[$i][0];
                            $count = $results[$i][2];
                            $inst = $results[$i][1];
                            
                          echo "<tr>
                            <td class='rank'>$one</td>
                            <td>$name</td>
                            <td>$inst</td>
                            <td>$count</td>
                          </tr>";
                          $one++;
                        }
                    }
                    else
                    {
                        $qry = "  SELECT AL.ACCNT_NAME NAM,AL.INSTITUTE_NAME INS,COUNT(*) CNT FROM CM_BATTLE_HISTORY CH,CM_ACCOUNT_LIST AL WHERE WINNER_ID IS NOT NULL AND WINNER_ID=AL.ACCNT_ID GROUP BY WINNER_ID,AL.ACCNT_NAME,AL.INSTITUTE_NAME ORDER BY COUNT(*) DESC";
                        $check = $conn->prepare($qry);
                        $check->execute();
                        $check_cnt = $check->rowCount();
                        $results = $check->fetchAll();
                        $one = 1;
                        for($i=0;$i<$check_cnt;$i++){
                            $name = $results[$i][0];
                            $count = $results[$i][2];
                            $inst = $results[$i][1];
                            
                          echo "<tr>
                            <td class='rank'>$one</td>
                            <td>$name</td>
                            <td>$inst</td>
                            <td>$count</td>
                          </tr>";
                          $one++;
                        }
                    }
                  ?>
                </table>
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