<?php
    require 'admin-header.php';
    
    
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
                        <label for="TypeName">Select User Type:</label>
                        <select name="TypeName" id="TypeName">
                            <option value="Admin">Admins</option>
                            <option value="User">Users</option>
                        </select>
                        <input type="submit" value="Submit" name="ins" />
                    </form>
                <!-- End -->
                </div>
                
                <table id="ranklist">
                
                  <tr>
                    <th class='rank'>SL.</th>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Password</th>
                    <th>Email</th>
                    <th>Details</th>
                  </tr>
                  <?php
                    if(isset($_GET["ins"]))
                    {
                        $type = $_GET['TypeName'];
                        if($type=="Admin")
                        {
                            $qry = "SELECT * FROM AmdinTable";
                            $check = $conn->prepare($qry);
                            $check->execute();
                            $check_cnt = $check->rowCount();
                            $results = $check->fetchAll();
                            $one = 1;
                            for($i=0;$i<$check_cnt;$i++){
                                $id = $results[$i][0];
                                $name = $results[$i][1];
                                $email = $results[$i][3];
                                $pass = $results[$i][4];
                                $details = $results[$i][5];
                                
                              echo "<tr>
                                <td class='rank'>$one</td>
                                <td>$id</td>
                                <td>$name</td>
                                <td>$pass</td>
                                <td>$email</td>
                                <td>$details</td>
                              </tr>";
                              $one++;
                            }
                        }
                        else
                        {
                            $qry = "SELECT * FROM UserTable";
                            $check = $conn->prepare($qry);
                            $check->execute();
                            $check_cnt = $check->rowCount();
                            $results = $check->fetchAll();
                            $one = 1;
                            for($i=0;$i<$check_cnt;$i++){
                                $id = $results[$i][0];
                                $name = $results[$i][1];
                                $email = $results[$i][2];
                                $pass = $results[$i][3];
                                $details = $results[$i][5];
                                
                              echo "<tr>
                                <td class='rank'>$one</td>
                                <td>$id</td>
                                <td>$name</td>
                                <td>$pass</td>
                                <td>$email</td>
                                <td>$details</td>
                              </tr>";
                              $one++;
                            }
                        }
                        
                    }
                    else
                    {
                        $qry = "SELECT * FROM AmdinTable";
                        $check = $conn->prepare($qry);
                        $check->execute();
                        $check_cnt = $check->rowCount();
                        $results = $check->fetchAll();
                        $one = 1;
                        for($i=0;$i<$check_cnt;$i++){
                            $id = $results[$i][0];
                            $name = $results[$i][1];
                            $email = $results[$i][3];
                            $pass = $results[$i][4];
                            $details = $results[$i][5];
                            
                          echo "<tr>
                            <td class='rank'>$one</td>
                            <td>$id</td>
                            <td>$name</td>
                            <td>$pass</td>
                            <td>$email</td>
                            <td>$details</td>
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
    require 'admin-footer.php';
?>