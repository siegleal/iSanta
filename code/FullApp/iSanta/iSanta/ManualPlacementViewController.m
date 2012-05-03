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



@interface ManualPlacementViewController()
@property (nonatomic, strong) NSMutableArray *ivArray;
@property (nonatomic, strong) UIActionSheet *modeSheet;
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


//bool deleting;
int status;
const int NORMAL = 0;
const int DELETING = 1;
const int MOVING = 2;

const double ALPHAVAL = .45;
const double ANIMATEDURATION = 1;
const int DISTTHRESHOLD = 30;


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
    
    
//    [self.scrollView addSubview:self.imageView];
    [self.scrollView addSubview:self.masterView];
    [self.masterView addSubview:self.imageView];
    self.navigationItem.rightBarButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Done" style:UIBarButtonItemStyleDone target:self action:@selector(donePlacing)];
//    deleting = NO;
   status = NORMAL;
    
    
}

- (void)viewWillDisappear:(BOOL)animated
{
    // Send points back to the detail view.
    [self.detailView setPoints:self.brain.points];
    // Do the super code
    [super viewWillDisappear:animated];
}
                                              

- (UIView *)viewForZoomingInScrollView:(UIScrollView *)scrollView{
    //return self.imageView;
        return self.masterView;
}

- (void)scrollViewDidEndZooming:(UIScrollView *)scrollView withView:(UIView *)view atScale:(float)scale{
    
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
    if (!_modeSheet) _modeSheet = [[UIActionSheet alloc] initWithTitle:@"Mode select" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Delete mode" otherButtonTitles:@"Edit mode", nil];
    return _modeSheet;
}


//this is a double tap
- (IBAction)tapped:(id)sender {
    CGPoint loc = [self.tapRecognizer locationInView:self.imageView];
    
    if (status == NORMAL){
        //add points to array
        [self.brain addPointatX:loc.x andY:loc.y];
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

- (void)actionSheet:(UIActionSheet *)actionsheet clickedButtonAtIndex:(NSInteger)index{

    switch (index) {
        case 0:
            status = DELETING;
           //dim background
            self.imageView.alpha = ALPHAVAL;
            //animate
            for (UIImageView *iv in self.ivArray) {
                [iv startAnimating];
            }
            break;
            
        case 1:
            status = MOVING;
            break;
            
        case 2:
            status = NORMAL;
            break;
            
        default:
            break;
    }
}

- (IBAction)longPress:(id)sender {
    if (status == NORMAL)
        [self.modeSheet showInView:self.view];
//
//
//    //start editing
//    //deleting = YES;
//    status = DELETING;
//    
//    //dim background
//    self.imageView.alpha = ALPHAVAL;
//
//    
//    //animate
//    for (UIImageView *iv in self.ivArray) {
//        [iv startAnimating];
//    }
//    
//    //let it be known that I am deleting
////    deleting = YES;
//    status = DELETING;
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
