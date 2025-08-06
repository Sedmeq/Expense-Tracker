# 💰 Expense Tracker - Personal Finance Management System

A modern and interactive expense tracking application built with ASP.NET Core 8.0 to simplify personal finance management.

## 🌟 Features

### 📊 Dashboard & Analytics
- **Real-time financial overview**: Income, expense, and balance statistics for the last 7 days
- **Interactive charts**: 
  - Expense breakdown by category (doughnut chart)
  - Income vs Expense trends (spline chart)
- **Recent transactions**: Quick view of latest financial activities

### 📝 Transaction Management
- **Full CRUD operations**: Complete transaction management functionality
- **Smart validation**: 
  - Prevents future date entries
  - Prevents dates older than 5 years
  - Amount and category validation
- **Category filtering**: Easy category selection with dropdown
- **Responsive grid**: Mobile-friendly transaction table

### 🏷️ Category Management
- **Dynamic categories**: Income and Expense category types
- **Icon support**: Visual representation with FontAwesome icons
- **Duplicate prevention**: Prevents creating categories with same names
- **Smart deletion**: Prevents deletion of categories with associated transactions

### 🎨 UI/UX Features
- **Modern dark theme**: Contemporary and eye-friendly design
- **Responsive design**: Perfect display across all devices
- **Interactive sidebar**: Dock/undock functionality
- **Real-time notifications**: Success/error message system
- **Syncfusion components**: Professional UI components

## 🛠️ Technologies

### Backend
- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core 9.0.7** - ORM
- **SQL Server LocalDB** - Database
- **Model validation** - Data annotation validation

### Frontend
- **Syncfusion EJ2** - UI components and charts
- **Bootstrap 5** - CSS framework
- **FontAwesome 6** - Icon library
- **jQuery** - JavaScript library
- **Inter Font** - Modern typography

### Database
- **Code-First approach** - EF migrations
- **Optimized indexing** - Performance indexes
- **Foreign key constraints** - Data integrity

## 📋 System Requirements

- **.NET 8.0 SDK** installed
- **SQL Server LocalDB** (comes with Visual Studio)
- **Modern web browser** (Chrome, Firefox, Edge, Safari)

## 🚀 Installation

### 1. Clone the repository
```bash
git clone [your-repository-url]
cd Expense-Tracker
```

## 🎯 Core Functionality

### Dashboard Analytics
- **Weekly overview**: Financial summary for the last 7 days
- **Visual analytics**: Interactive charts and graphs
- **Quick insights**: Balance, income, and expense indicators

### Category Management
- **Flexible categorization**: Unlimited number of categories
- **Icon integration**: Visual identification with icons
- **Type classification**: Income and Expense categories

### Transaction Tracking
- **Detailed records**: Date, amount, note, and category
- **Smart filtering**: Category and date filtering
- **Bulk operations**: Management of multiple transactions

## 🔧 Configuration

### Database Connection
Connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ExpenseTrackerDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### Syncfusion License
Syncfusion license is configured in `Program.cs`.

## 📱 Responsive Design

The application works optimally on the following screen sizes:
- **Mobile**: 320px - 768px
- **Tablet**: 768px - 1024px  
- **Desktop**: 1024px+

## 🔒 Security Features

- **CSRF Protection**: Anti-forgery tokens
- **Input Validation**: Server-side validation
- **SQL Injection Prevention**: EF Core parameterized queries
- **XSS Protection**: HTML encoding

## 🐛 Debug & Logging

- **ILogger integration**: Structured logging
- **Error handling**: Global exception handling
- **Debug information**: Detailed errors for development environment

## 🚀 Production Deployment

### For IIS deployment:
1. Create publish profile
2. Run `dotnet publish -c Release`
3. Create application pool in IIS (.NET 8.0)
4. Update connection string for SQL Server

## 📈 Performance Optimizations

- **Database indexing**: For frequently accessed fields
- **Lazy loading**: For navigation properties
- **Async operations**: All database operations
- **Caching**: Browser caching for static assets


## 🎯 Future Enhancements

- **Export functionality**: PDF/Excel reports
- **Budget planning**: Monthly budget tracking
- **Multi-currency support**: International currency handling
- **Mobile app**: React Native or Flutter mobile version
- **Cloud sync**: Online backup and synchronization

### Test Coverage
- Unit tests for controllers
- Integration tests for database operations
- UI tests for critical user flows

### 🎉 Final Notes

This application is designed for personal finance management and can be used for learning purposes by both developers and end-users. Modern web development best practices have been implemented throughout the project.

**Happy budgeting! 💰📊**
