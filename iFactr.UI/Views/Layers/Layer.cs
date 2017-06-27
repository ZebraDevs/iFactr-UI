namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents the base implementation of an iFactr navigation layer.  This class is abstract.
    /// </summary>
    /// <remarks>
    /// A <b>Layer</b> is comprised of navigation items, (lists and menus), and items
    /// used for display of information, (blocks and panels). A Navigation Layer's
    /// primary purpose is to provide information and workflow options within your
    /// application. 
    /// <para> </para>
    /// <para><b>Designing iFactr Layers</b> </para>
    /// <para>Designing your layers should follow a disciplined process of
    /// de-constructing your application domain entities into defined workflows that can
    /// be easily translated across target platforms. For example, in a simple order
    /// entry system you will likely need to model the concept of an Order with multiple
    /// Order Line-items. A clean de-construction of these concepts would likely result
    /// in the design of at least three iFactr Layers: </para>
    /// <para> </para>
    /// <list type="bullet">
    /// <item>
    /// <description>An Order list layer, which displays a list of orders.
    /// </description></item>
    /// <item>
    /// <description>An Order detail layer, which describes the contents of the order
    /// including display, and addition of Order Line-items. </description></item>
    /// <item>
    /// <description>A new Order form, which captures information for a new order.
    /// </description></item></list>
    /// <para> </para>
    /// <para>Each of these layers represents a discrete piece of functionality which
    /// can be accessed and run independently depending on the workflow specified in
    /// your application requirements. Creation of a new order may be requested from the
    /// main menu, or navigation tabs at the top-level of your application, but it may
    /// also be accessed from your application's product list. Both of these workflows
    /// can be easily supported by re-combining existing layers in your application's
    /// Navigation Map. </para>
    /// <para> </para>
    /// <para><b>Adding Layers to the Navigation Map</b> </para>
    /// <para>By carefully constructing your Navigation Map to model your application's
    /// many workflows, you can support even the most complex use cases using the simple
    /// functionality represented in your application Layers. Each Layer in your
    /// application can have multiple endpoints defined in your Navigation Map. These
    /// endpoints will reflect the workflow for each use case defined in your
    /// application requirements to maximize re-use of existing layer functionality
    /// wherever possible </para>
    /// <para> </para>
    /// <para>Now let's consider another likely scenario where we want to create a new
    /// order for an existing customer. We want to re-use the existing OrderDetails
    /// layer, since it contains all of the order information we want to interact with.
    /// So I'll add the following endpoint to my Navigation Map: </para>
    /// <para> </para>
    /// <para><c>NavigationMap.Add(&quot;Orders/Add/{Customer}&quot;, orderDetails);</c> </para>
    /// <para> </para>
    /// <para>Now when I request my layer, I'll be passing the Customer ID of the
    /// customer for whom I want to create a new order. So a URI of
    /// &quot;Orders/Add/9876&quot; passed to my application will load the same
    /// OrderDetails layer as before, but this time I will have a different parameter in
    /// my parameters dictionary: Customer=9876. </para>
    /// <para> </para>
    /// <para>I can then apply the appropriate logic to create and save a new Order for
    /// the customer: </para>
    /// <para> </para>
    /// <code lang="C#">     class OrderDetails : iLayer
    ///      {
    ///          public override void Load(Dictionary&lt;string, string&gt; parameters)
    ///          {
    ///             string Id = null;
    /// 
    ///             if (parameters.ContainsKey(&quot;Id&quot;))
    ///                     Id = parameters[&quot;Id&quot;];
    /// 
    ///             if (Id != null)
    ///             {
    ///                 Data.Order order =</code>
    /// <para> </para>
    /// <code lang="C#">
    /// Data.Providers.ProviderMap.Orders.PendingOrders.Where(item =&gt;
    ///                                 item.OrderId.ToString().Substring(1, 5) ==
    /// Id).FirstOrDefault();
    ///             }
    /// 
    ///             if (Id == null &amp;&amp;
    /// parameters.ContainsKey(&quot;Customer&quot;))
    ///             {
    ///                      string CustId = parameters[&quot;Customer&quot;];
    /// 
    ///                      Data.Order newOrder = new Data.Order();
    ///                      newOrder.Customer =
    /// 
    /// Data.Providers.ProviderMap.Customers.CacheList.Where(customer =&gt;
    ///                             customer.ID.Trim() == CustId).FirstOrDefault();
    /// 
    ///                      Id = newOrder.OrderId.ToString().Substring(1,5);
    /// 
    /// 
    /// Data.Providers.ProviderMap.Orders.PendingOrders.Add(newOrder);
    ///                      Data.Providers.ProviderMap.Orders.SavePendingOrders();
    ///                </code>
    /// <para> </para>
    /// <code lang="C#">            }
    ///             .
    ///             .
    ///             .
    ///         }
    ///      }</code>
    /// <para> </para>
    /// <para> </para>
    /// <para>To add additional endpoints, simply follow your additional usage scenarios
    /// logical flow. </para>
    /// <para> </para>
    /// <para>To add an item to an existing order: </para>
    /// <para> </para>
    /// <para> <c>NavigationMap.Add(&quot;Orders/{Id}/NewItem/{Product}&quot;, orderDetails);  </c></para>
    /// <para> </para>
    /// <para>To create a new order, and add an item: </para>
    /// <para> </para>
    /// <para> <c> NavigationMap.Add(&quot;Orders/New/{Customer}/{Product}&quot;, orderDetails); </c> </para>
    /// <para> </para>
    /// <para>Simply add logic to your Layer to handle each scenario according to the
    /// array of parameters passed when the layer is loaded. </para>
    /// <para> </para>
    /// <para> </para>
    /// <para><b>The Layer Parameters Dictionary</b> </para>
    /// <para>The iFactr framework automatically processes all parameters in the navigation URI and delivers them to the <c>Layer.OnLoad()</c> method for processing. Parameters are passed as a dictionary of strings, with a string for a key. These values are derived from the navigation URI of the Layer request, and the corresponding URI in your application's navigation map. </para>
    /// <para> </para>
    /// <para>The purpose of the Layer parameters dictionary is to provide the necessary
    /// context for layer processing. Object identifiers and other metadata should be
    /// parameterized in your navigation map, and substituted at runtime for individual
    /// use cases. </para>
    /// <para> </para>
    /// <para>he iFactr Framework uses this navigation-centric approach to establish
    /// multiple workflows. The Navigation Map is defined on your base application, and
    /// contains all references to your application Layers. Using our hypothetical
    /// order-entry system, we will add a reference in our navigation map for our
    /// OrderList layer as follows: </para>
    /// <para> </para>
    /// <code lang="C#">     iLayer orders = new OrderList();
    ///      NavigationMap.Add(&quot;Orders&quot;, orders);</code>
    /// <para> </para>
    /// <para>This provides our base entry-point to the <c>OrderList</c> layer. Now whenever the URI <c>&quot;Orders&quot;</c> is passed to the iFactr navigation framework, the <c>OrderList</c> layer will be processed and display the list of orders. </para>
    /// <para> </para>
    /// <para>Now let's suppose I want to view the details of an individual order. I
    /// will add an endpoint as follows to my Navigation Map: </para>
    /// <para> </para>
    /// <code lang="C#">     iLayer orderDetails = new OrderDetails();
    ///      NavigationMap.Add(&quot;Orders/{Id}&quot;, orderDetails);</code>
    /// <para> </para>
    /// <para> </para>
    /// <para>This establishes the default entry-point for an individual order. This URI
    /// template uses the squiggly bracket syntax to indicate a substituted value, (in
    /// this case, the Order ID), in the navigation URI. At the same time, I will create
    /// my list items on my layer to substitute the Order ID for the {} parameter in the
    /// navigation map URI: </para>
    /// <para> </para>
    /// <code lang="C#">    class OrderList : iLayer
    ///     {
    ///         public override void Load(Dictionary&lt;string, string&gt; parameters)
    ///         {
    ///             .
    ///             .
    ///             .
    ///             foreach (Data.Order order in
    ///                      Data.Providers.ProviderMap.Orders.PendingOrders)
    ///             {
    ///                 plist.Items.Add(new iItem(&quot;Orders/&quot; +
    ///                                       order.OrderId.ToString(),
    ///                                       order.Customer.Name.ToTitleCase(),
    ///                                       string.Format(&quot;Items:{0}&quot;,
    ///                                       order.Items.Count.ToString()),true));
    ///             }
    ///             .
    ///             .
    ///             .
    ///         }
    ///     }</code>
    /// <para> </para>
    /// <para>The result is a list with individual endpoints, (i.e. <c>&quot;Orders/1234&quot;</c>, <c>&quot;Orders/5678&quot;</c>, etc.), for each item on my list. </para>
    /// <para> </para>
    /// <para>At runtime, the navigation framework will parse this URI to extract the individual Order ID and place it in the layer parameters dictionary for processing. So a URI of <c>&quot;Orders/1234&quot;</c> passed to the <c>iApp.Navigate()</c> method will load the <c>OrderDetails</c> layer, passing a single parameter: <c>Id=1234</c>. </para>
    /// <para> </para>
    /// <para>I can now use this parameter to retrieve the order details for display:
    /// </para>
    /// <para> </para>
    /// <code lang="C#">   class OrderDetails : iLayer
    ///    {
    ///         public override void Load(Dictionary&lt;string, string&gt; parameters)
    ///         {
    ///               string Id = null;
    /// 
    ///               if (parameters.ContainsKey(&quot;Id&quot;))
    ///                      Id = parameters[&quot;Id&quot;];
    /// 
    ///               if (Id != null)
    ///               {
    ///                     Data.Order order =
    /// 
    ///                     Data.Providers.ProviderMap.Orders.PendingOrders.Where(item
    /// =&gt;
    ///                              item.OrderId.ToString().Substring(1, 5) ==
    ///                                                      Id).FirstOrDefault();
    ///               }
    ///             .
    ///             .
    ///             .
    ///         }
    ///     }</code>
    /// <para> </para>
    /// <para> </para>
    /// <para> </para>
    /// <para><b>Proper Use of the Parameters Dictionary</b> </para>
    /// <para>The Layer parameter dictionary's purpose is to provide only the metadata
    /// necessary to process your layers according to your business workflows as defined
    /// in the navigation map. The parameters dictionary is created and managed by the
    /// iFactr framework; as such any parameters added outside of the scope of the URI
    /// templates in the navigation map are not guaranteed to be present in your layer
    /// processing. <b><i>It should not be used to pass business domain objects, or
    /// other information gathered and processed in application code.</i></b></para>
    /// <para> </para>
    /// </remarks>
    public abstract class Layer : iLayer
    {
    }
}