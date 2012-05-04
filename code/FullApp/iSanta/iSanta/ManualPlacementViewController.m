//
//  ManualPlacementViewController.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "ManualPlacementViewController.h"
#import "DetailViewController.h"

//
//@interface myUIImageView : UIImageView
//
//@property (nonatomic) CGFloat alpha;
//@end
//
//@implementation myUIImageView
//
//- (void) setAlpha:(CGFloat)alpha{
//    
//}
//
//-(CGFloat) alpha{
//    return (CGFloat)1.0;
//}
//
//@end

//@implementation mySingleTapRec
//
//- (void) touchesMoved:(NSSet *)touches withEvent:(UIEvent *)event
//{
//    NSLog(@"moved");
//}
//
//@end






@interface ManualPlacementViewController()
@property (nonatomic, strong) NSMutableArray *ivArray;
@property (nonatomic, strong) UIActionSheet *modeSheet;
@property (nonatomic, strong) UIButton *upButton;
@property (nonatomic, strong) UIButton *dnButton;
@property (nonatomic, strong) UIButton *lfButton;
@property (nonatomic, strong) UIButton *rtButton;
@property (nonatomic, strong) UIImage *upImage;
@property (nonatomic, strong) UIImage *dnImage;
@property (nonatomic, strong) UIImage *lfImage;
@property (nonatomic, strong) UIImage *rtImage;

@property (nonatomic, strong) UIImageView *selectedPoint;
- (void)buttonsVisible:(BOOL) vis;
@end



@implementation ManualPlacementViewController
@synthesize brain = _brain;
@synthesize tapRecognizer = _tapRecognizer;
@synthesize longPressRec = _longPressRec;
@synthesize singleTapRec = _singleTapRec;
@synthesize scrollView = _scrollView;
@synthesize masterView = _masterView;
@synthesize imageView = _imageView;
@synthesize ivArray = _ivArray;
@synthesize modeSheet = _modeSheet;
@synthesize detailView = _detailView;
@synthesize upButton = _upButton;
@synthesize dnButton = _dnButton;
@synthesize lfButton = _lfButton;
@synthesize rtButton = _rtButton;
@synthesize upImage = _upImage;
@synthesize dnImage = _dnImage;
@synthesize lfImage = _lfImage;
@synthesize rtImage = _rtImage;
@synthesize selectedPoint = _selectedPoint;



//bool deleting;
int status;
const int NORMAL = 0;
const int DELETING = 1;
const int MOVING = 2;

const double ALPHAVAL = .45;
const double ANIMATEDURATION = 1;
const int DISTTHRESHOLD = 30;
const int EDITTHRESHOLD = 20;

const int BTNSMALL = 60;
const int BTNBIG = 125;
const double BTNALPHA = .5;


int currentOp = 1;

- (void) donePlacing{
    NSLog(@"you continued");
}

- (void) viewDidLoad
{
    self.imageView.image = self.brain.targetImage;
    self.view.backgroundColor = [UIColor blackColor];
    [self.view addGestureRecognizer:[self tapRecognizer]];
    [self.view addGestureRecognizer:[self longPressRec]];
    [self.view addGestureRecognizer:[self singleTapRec]];
    
    
    self.scrollView.minimumZoomScale=1.0;
    self.scrollView.maximumZoomScale = 3.0;
    self.scrollView.delegate = self;
    
    self.masterView.backgroundColor = [UIColor blackColor];
    

    [self.scrollView addSubview:self.masterView];
    [self.masterView addSubview:self.imageView];
    self.navigationItem.rightBarButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Done" style:UIBarButtonItemStyleDone target:self action:@selector(donePlacing)];

   status = NORMAL;
    
    [self.scrollView addSubview:self.upButton];
    [self.scrollView addSubview:self.dnButton];
    [self.scrollView addSubview:self.lfButton];
    [self.scrollView addSubview:self.rtButton];
    
    [self.upButton setImage:self.upImage forState:UIControlStateNormal];
    [self.dnButton setImage:self.dnImage forState:UIControlStateNormal];
    [self.lfButton setImage:self.lfImage forState:UIControlStateNormal];
    [self.rtButton setImage:self.rtImage forState:UIControlStateNormal];
    
    self.upButton.hidden = YES;
    self.dnButton.hidden = YES;
    self.lfButton.hidden = YES;
    self.rtButton.hidden = YES;

    
}

- (UIImage *) upImage{
    if (!_upImage) _upImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"btn_up" ofType:@"png"]];
    return _upImage;
}
- (UIImage *) dnImage{
    if (!_dnImage) _dnImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"btn_dn" ofType:@"png"]];
    return _dnImage;
}
- (UIImage *) lfImage{
    if (!_lfImage) _lfImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"btn_lf" ofType:@"png"]];
    return _lfImage;
}
- (UIImage *) rtImage{
    if (!_rtImage) _rtImage = [[UIImage alloc] initWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"btn_rt" ofType:@"png"]];
    return _rtImage;
}

- (UIButton *) upButton{
    if (!_upButton) {
        _upButton = [[UIButton alloc] initWithFrame:CGRectMake(self.view.center.x - (BTNBIG / 2), 0.0, BTNBIG,BTNSMALL)];
        self.upButton.backgroundColor = [UIColor grayColor];
        _upButton.alpha = BTNALPHA;
        
    }
    return _upButton;
}
- (UIButton *) dnButton{
    if (!_dnButton) {
        _dnButton = [[UIButton alloc] initWithFrame:CGRectMake(self.view.center.x - (BTNBIG / 2), self.view.bounds.size.height - BTNSMALL, BTNBIG,BTNSMALL)];
        self.dnButton.backgroundColor = [UIColor grayColor];
        _dnButton.alpha = BTNALPHA;
         
    }
    return _dnButton;
}
- (UIButton *) lfButton{
    if (!_lfButton) {
        _lfButton = [[UIButton alloc] initWithFrame:CGRectMake(0, self.view.center.y - (BTNBIG/ 2), BTNSMALL,BTNBIG)];
        self.lfButton.backgroundColor = [UIColor grayColor];
        _lfButton.alpha = BTNALPHA;
            }
    return _lfButton;
}
- (UIButton *) rtButton{
    if (!_rtButton) {
        _rtButton = [[UIButton alloc] initWithFrame:CGRectMake(self.view.bounds.size.width - BTNSMALL, self.view.center.y - (BTNBIG/2), BTNSMALL,BTNBIG)];
        self.rtButton.backgroundColor = [UIColor grayColor];
        _rtButton.alpha = BTNALPHA;
           }
    return _rtButton;
}
                                              

- (UIView *)viewForZoomingInScrollView:(UIScrollView *)scrollView{
    //return self.imageView;
        return self.masterView;
}

- (void) centerButtons{
    [self.upButton setFrame:CGRectMake(self.scrollView.center.x + self.scrollView.contentOffset.x - (.5 * BTNBIG), self.scrollView.bounds.origin.y,self.upButton.bounds.size.width,self.upButton.bounds.size.height)];
    [self.dnButton setFrame:CGRectMake(self.view.center.x + self.scrollView.contentOffset.x - (BTNBIG / 2), self.scrollView.bounds.size.height + self.scrollView.contentOffset.y - BTNSMALL, BTNBIG,BTNSMALL)];
    [self.lfButton setFrame:CGRectMake(self.scrollView.bounds.origin.x, self.view.center.y + self.scrollView.contentOffset.y - (BTNBIG/ 2), BTNSMALL,BTNBIG)];
    [self.rtButton setFrame:CGRectMake(self.view.bounds.size.width + self.scrollView.contentOffset.x - BTNSMALL, self.view.center.y + self.scrollView.contentOffset.y - (BTNBIG/2), BTNSMALL,BTNBIG)];
}

- (void)scrollViewDidEndZooming:(UIScrollView *)scrollView withView:(UIView *)view atScale:(float)scale{
    [self centerButtons];
}

- (void)scrollViewDidEndScrollingAnimation:(UIScrollView *) scrollView{
    [self centerButtons];
}

- (void)scrollViewDidEndDecelerating:(UIScrollView *) scrollView{
    [self centerButtons];
}

- (void)scrollViewDidScroll:(UIScrollView *) scrollView{
    [self centerButtons];
}

- (NSArray *)ivArray{
    if (!_ivArray) _ivArray = [[NSMutableArray alloc] init];
    return _ivArray;
}

- (PlacementBrain *)brain
{
    if (!_brain) _brain = [[PlacementBrain alloc] init];
    return _brain;
}

- (UIView *)masterView{
    if (!_masterView) _masterView = [[UIView alloc] init];
    return _masterView;
}

-(UIActionSheet *)modeSheet{
    if (!_modeSheet) _modeSheet = [[UIActionSheet alloc] initWithTitle:@"Mode select" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Delete mode" otherButtonTitles:@"Edit mode", @"Normal mode", nil];
    return _modeSheet;
}


//this is a double tap
- (IBAction)tapped:(id)sender {
    CGPoint loc = [self.tapRecognizer locationInView:self.imageView];
    
    if (status == NORMAL){
        //add points to array
        [self.brain addPointatX:loc.x andY:loc.y];
        //add point to the detail view and coredata.
        [self.detailView addPointWithXValue:loc.x andYValue:loc.y];
        //draw them 
        UIImageView *iv = [[UIImageView alloc] initWithImage:self.brain.circleImage];
        iv.animationImages = self.brain.animationArray;
        iv.center = loc;
        iv.animationDuration = ANIMATEDURATION;
//        [self.imageView addSubview:iv];
        [self.masterView addSubview:iv];
        //add view to array
        [self.ivArray addObject:iv];
    }
    else //double tap to go back to NORMAL
    {
        //done deleting
//        deleting = NO;
        status = NORMAL;
        
        //undim
//        self.middleView.alpha = 0.0;
      self.imageView.alpha = 1.0;
        
        //stop animating
        for (UIImageView *iv in self.ivArray) {
            [iv stopAnimating];
        }

    }
}

- (void)buttonsVisible:(BOOL) vis{
    self.upButton.hidden = vis;
    self.dnButton.hidden = vis;
    self.lfButton.hidden = vis;
    self.rtButton.hidden = vis;

}

- (void)actionSheet:(UIActionSheet *)actionsheet clickedButtonAtIndex:(NSInteger)index{

    switch (index) {
        case 0:
            status = DELETING;
            [self buttonsVisible:(YES)];

           //dim background
            self.imageView.alpha = ALPHAVAL;
            
            //animate
            for (UIImageView *iv in self.ivArray) {
                [iv startAnimating];
            }
            break;
            
        case 1:
            status = MOVING;
             [self buttonsVisible:(NO)];

            break;
            
        case 2:
            status = NORMAL;
             [self buttonsVisible:(YES)];

            break;
            
        default:
            break;
    }
}

- (IBAction)longPress:(id)sender {
    //if (status == NORMAL)
        [self.modeSheet showInView:self.view];

}






- (IBAction)singleTap:(id)sender {
    if (status == DELETING)
    {
        CGPoint loc = [self.singleTapRec locationInView:self.imageView];
        
        //find the closest point
        int closestIndex = 0;
        double closestDist = 999;

        if (self.brain.points.count > 0)
        {
            //loop thru all of the points
            NSEnumerator *e = [self.brain.points objectEnumerator];            
            for (int i = 0; i < self.brain.points.count; i++) 
            {
                NSValue *obj = [e nextObject];
                double dist = sqrt(pow(loc.x - obj.CGPointValue.x,2.0) + pow(loc.y - obj.CGPointValue.y,2.0));
                if (dist < closestDist) {
                    closestDist = dist;
                    closestIndex = i;
                }
            }
            
            //prevent a random tap from removing a point
            if (closestDist < DISTTHRESHOLD){
                //remove from controller & view
                UIImageView *iv = [self.ivArray objectAtIndex:closestIndex];
                [iv removeFromSuperview];
                [self.ivArray removeObjectAtIndex:closestIndex];
                
                
                //remove from brain            
                [self.brain removePoint:[self.brain.points objectAtIndex:closestIndex]];

            }
        }
        

    }
    else if (status == MOVING) {
        CGPoint loc = [self.singleTapRec locationInView:self.masterView];
        
        //find the closest point
        int closestIndex = 0;
        double closestDist = 999;
        
        if (self.brain.points.count > 0)
        {
            //loop thru all of the points
            NSEnumerator *e = [self.brain.points objectEnumerator];            
            for (int i = 0; i < self.brain.points.count; i++) 
            {
                NSValue *obj = [e nextObject];
                double dist = sqrt(pow(loc.x - obj.CGPointValue.x,2.0) + pow(loc.y - obj.CGPointValue.y,2.0));
                if (dist < closestDist) {
                    closestDist = dist;
                    closestIndex = i;
                }
            }
            
            
            if (closestDist < EDITTHRESHOLD){
                double zoomFactor = 2.0;
                //remove from controller & view
                if (self.selectedPoint)
                    [self.selectedPoint setImage:self.brain.circleImage];
                self.selectedPoint = [self.ivArray objectAtIndex:closestIndex];
                [self.selectedPoint setImage:self.brain.editImage];
                [self.scrollView setZoomScale:zoomFactor];
                [self.scrollView setContentOffset:CGPointMake((zoomFactor * self.selectedPoint.center.x) - (.5 * self.scrollView.bounds.size.width), (zoomFactor * self.selectedPoint.center.y) - (.5 * self.scrollView.bounds.size.height)) animated:YES];
                //[self.scrollView setContentOffset:CGPointMake(self.imageView.center.x - (self.scrollView.center.x - self.selectedPoint.center.x), self.imageView.center.y - (self.scrollView.center.y - self.selectedPoint.center.y)) animated:YES];

                
            }
            else {
                if (self.selectedPoint)
                    [self.selectedPoint setImage:self.brain.circleImage];
                [self.scrollView setZoomScale:1.0];
                [self.scrollView setContentOffset:CGPointMake(0, 0 )];
            }
        }
    }
        
}


- (UITapGestureRecognizer *) tapRecognizer{
    if (!_tapRecognizer) _tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(tapped:)];
    _tapRecognizer.numberOfTapsRequired = 2;
    return _tapRecognizer;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

- (void)viewDidUnload {
    [self setImageView:nil];
    [self setTapRecognizer:nil];
    [self setLongPressRec:nil];
    [self setSingleTapRec:nil];
    [self setScrollView:nil];
    [self setMasterView:nil];
    [super viewDidUnload];
}
@end
