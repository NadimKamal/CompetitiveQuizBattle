Imports Microsoft.VisualBasic

Public Class clsFinancial

    Public Function CalculatePMT(ByVal dblPV As Double, ByVal dblRate As Double, ByVal dblNPer As Double, ByVal dblFV As Double, ByVal intAdd_Arr As Integer) As Double
        Dim dblRental As Double

        If intAdd_Arr = 1 Then
            dblRental = Financial.Pmt(dblRate, dblNPer, (-1 * dblPV), dblFV, DueDate.BegOfPeriod)
        Else
            dblRental = Financial.Pmt(dblRate, dblNPer, (-1 * dblPV), dblFV, DueDate.EndOfPeriod)
        End If
        Return dblRental
    End Function

    Public Function CalculateIMPT(ByVal dblPV As Double, ByVal dblRate As Double, ByVal dblPer As Double, ByVal dblNPer As Double, ByVal dblFV As Double, ByVal intAdd_Arr As Integer) As Double
        Dim dblRental As Double

        If intAdd_Arr = 1 Then
            dblRental = Financial.IPmt(dblRate, dblPer, dblNPer, (-1 * dblPV), dblFV, DueDate.BegOfPeriod)
        Else
            dblRental = Financial.IPmt(dblRate, dblPer, dblNPer, (-1 * dblPV), dblFV, DueDate.EndOfPeriod)
        End If
        Return dblRental
    End Function

End Class
