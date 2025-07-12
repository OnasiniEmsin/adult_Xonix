using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xonix : MonoBehaviour
{
    public GameObject playerpref,enemy;
    public Transform canvasparent;
    public int[,] koord;
    public int ustun=4,satr=4;
    public int ekranx=1920,ekrany=1080;
    public int x,y,ix,iy,maxx=0,ymax=0,minx,ymin,summa,jon,foiz=100;
    public float katakx,kataky;
    bool ispainting,isbrushing;
    public GridLayoutGroup gridLayoutGroup;
    public GameObject prefabButton;
    public const int chegara=11,chiziq=2,polya=0,boshjoy=3,chapyon=-1,ongyon=1;
    public List<Image> images;
    Transform mytr;
    Coroutine co;
    void addbatn()
    {
        


        minx=satr;ymin=ustun;
        images=new List<Image>();
        for (int i = 0; i <satr; i++)
        {
            for (int j = 0; j < ustun; j++){
                GameObject newButton = Instantiate(prefabButton, gridLayoutGroup.transform);
                images.Add(newButton.GetComponent<Image>());
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        katakx=(float)Screen.width/ustun;kataky=(float)Screen.height/satr;
        koord=new int[satr,ustun];
        gridLayoutGroup.GetComponent<GridLayoutGroup>().cellSize=new Vector2(katakx,kataky);
        mytr = Instantiate(enemy, canvasparent).transform;
        enemy enem=mytr.GetComponent<enemy>();
        enem.gameManager = this;
        enem.x=5;enem.y=5;
        
        mytr.transform.localPosition=new Vector2((satr/2)*katakx,(-ustun/2)*-kataky);
        /*RectTransform rt = mytr.GetComponent<RectTransform>();

        // Anchor: yuqori chap burchak
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);*/
        addbatn();
        boya(satr,ustun);
        co=StartCoroutine(Xonix());
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
    void harakat(){
        ix=(int)Input.GetAxisRaw("Horizontal");
        iy=(int)Input.GetAxisRaw("Vertical");
        iy*=-1;
        if(ix!=0){
            x+=ix;iy=0;
        }else{
            if(iy!=0){
                y+=iy;ix=0;
            }
        }
        if(y<0){
            y=0;
        }else{
            if(y>=ustun){
                y=ustun-1;
            }
        }
        if(x<0){
            x=0;
        }else{
            if(x>=satr){
                x=satr-1;
            }
        }
        tekshir();
    }
    void tekshir(){
        //mytr.transform.localPosition=new Vector2((-satr/2+x)*katakx,(-ustun/2+y)*-kataky);
        switch(koord[x,y]){
            case polya:koord[x,y]=chiziq;isbrushing=true;maxx=x;ymax=y;minx=x;ymin=y;break;
            case chegara:isBrushed();break;
            case boshjoy:isBrushed();break;
            case chiziq:minus1();break;
        }
        
        //x+=ix;y+=iy;
    }
    public void minus1(){
        if(jon>0){
            jon--;
            boya(satr,ustun);
        }else{
            gameOver();
        }
    }
    void isBrushed(){
        if(isbrushing){
            isbrushing=false;
            
            int h=minx,k=ymin;
            narmalniychiziq(h,k);
     
        }
    }
    void narmalniychiziq(int u,int v){
        if(koord[u-1,v]==polya&&koord[u+1,v]==polya){
            ochir(u-1,v,chapyon);
            ochir(u+1,v,ongyon);
        }else{
            if(koord[u,v-1]==polya&&koord[u,v+1]==polya){
                ochir(u,v-1,chapyon);
                ochir(u,v+1,ongyon);
            }else{
                koord[u,v]=boshjoy;
                if(koord[u,v-1]==chiziq){
                    narmalniychiziq(u,v-1);
                }
                if(koord[u,v+1]==chiziq){
                    narmalniychiziq(u,v+1);
                }
                if(koord[u-1,v]==chiziq){
                    narmalniychiziq(u-1,v);
                }
                if(koord[u+1,v]==chiziq){
                    narmalniychiziq(u+1,v);
                }
                
            }
        }
    }
    void boya(int a,int b){
        x=satr/2;y=ustun;
        isbrushing=false;
        for(int i=0;i<a;i++){
            for(int j=0;j<b;j++){
                if(i==0||j==0||i==satr-1||j==ustun-1){
                    koord[i,j]=chegara;
                }else{
                    koord[i,j]=polya;
                }
            }   
        }
    }
    void ochir(int i,int j,int k){
        koord[i,j]=k;
        summa+=k;
        if(koord[i+1,j]==polya){
            ochir(i+1,j,k);
        }
        if(koord[i-1,j]==polya){
            ochir(i-1,j,k);
        }
        if(koord[i,j+1]==polya){
            ochir(i,j+1,k);
        }
        if(koord[i,j-1]==polya){
            ochir(i,j-1,k);
        }
        if(koord[i+1,j]==chiziq){
            ochirch(i+1,j);
        }
        if(koord[i-1,j]==chiziq){
            ochirch(i-1,j);
        }
        if(koord[i,j+1]==chiziq){
            ochirch(i,j+1);
        }
        if(koord[i,j-1]==chiziq){
            ochirch(i,j-1);
        }
        maxx=ymax=0;
        minx=satr;ymin=ustun;
    }
    void isLine(int a,int b){
        if(ispainting){
            switch(koord[a,b]){
                case polya:koord[a,b]=boshjoy;break;
                case chiziq:ispainting=false;koord[a,b]=boshjoy;break;
            }
        }
    }
    void Repaint(){
        int a=0,b=0,ss=0;
        int ustuvor=boshjoy,fi;
        if(summa<0){
            ustuvor=ongyon;
            summa=0;
        }else{
            ustuvor=chapyon;
            summa=0;
        }
        float foiz1=0;
        foreach (var img in images){
            a=ss-(ss/ustun)*ustun;
            b=ss/ustun;
            ss++;
            fi=koord[a,b];
            if(fi==ustuvor){
                koord[a,b]=boshjoy;
                fi=boshjoy;
            }
            if(fi==-ustuvor){
                koord[a,b]=polya;
                
            }
            switch(fi){
                case polya:img.color=new Color(1,1,1,1);foiz1++;break;
                case chiziq:img.color=new Color(1,0,0,1);break;
                case boshjoy:img.color=new Color(0,0,0,0);break;
                case chegara:img.color=new Color(1,0,1,1);foiz1++;break;
                
            }
        }
        foiz=(int)(foiz1/satr/ustun*100);
    }
    IEnumerator Xonix(){
        while(true){
        yield return new WaitForSeconds(.03f);
        harakat();
        Repaint();
        }
    }
    void ochirch(int i,int j){
        koord[i,j]=boshjoy;
        if(koord[i+1,j]==chiziq){
            ochirch(i+1,j);
        }
        if(koord[i-1,j]==chiziq){
            ochirch(i-1,j);
        }
        if(koord[i,j+1]==chiziq){
            ochirch(i,j+1);
        }
        if(koord[i,j-1]==chiziq){
            ochirch(i,j-1);
        }
        maxx=ymax=0;
        minx=satr;ymin=ustun;
    }
    void gameOver(){
        StopCoroutine(co);
        Debug.Log("Game Over");
    }
}
